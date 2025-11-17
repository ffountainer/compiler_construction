using System.Collections;
using compiler_construction.Interpretation;
using compiler_construction.Semantics;
using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class ReferenceNode : TreeNode
{
    private bool calledByForHeader;

    private WhatReference whatReference;
    
    private WhatTupleReference whatTupleReference;

    public WhatTupleReference GetWhatTupleReference()
    {
        return whatTupleReference;
    }
    
    private IdentifierNode identifier;

    private ExpressionNode arrayIndex;
    
    public ExpressionNode GetArrayIndex() => arrayIndex;
    
    private IdentifierNode tupleAccessIdentifier;
    
    public IdentifierNode GetTupleAccessIdentifier() => tupleAccessIdentifier;

    private int tupleIndexInt;
    
    public int GetTupleIndexInt() => tupleIndexInt;
    
    public IdentifierNode GetIdentifier()
    {
        return identifier;
    }
    
    public WhatReference getWhatReference() => whatReference;

    public ReferenceNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Reference";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        Debug.Log($"Start constructing reference from {firstToken.GetSourceText()}");
        
        if (firstToken is not Identifier)
        {
            throw new UnexpectedTokenException("Expected identifier but got " + firstToken);
        }
        
        var ident = NodeFactory.ConstructNode(new IdentifierNode(), lexer, firstToken);
        identifier = ident;
        children.Add(ident);
        
        bool flag = false;
        var check_scope = SyntaxAnalyzer.GetCurrentScope();
        do
        {
            if (check_scope.GetScope().ContainsKey(firstToken.GetSourceText()) || calledByForHeader)
            {
                flag = true;
            }

            check_scope = check_scope.GetParentScope();
        } while (check_scope != null && flag == false);
            
        if (!flag)
        {
            throw new SemanticException($"No such variable declared \"{firstToken.GetSourceText()}\"");
        }

        var opToken = lexer.GetNextToken();

        flag = false;
        
        check_scope = SyntaxAnalyzer.GetCurrentScope();
        do
        {
            if (!(SyntaxAnalyzer.GetCurrentScope().GetScope()[firstToken.GetSourceText()] is false && (IsAssign == false || !currentIdent.Equals(firstToken.GetSourceText())) && !opToken.GetSourceText().Equals("is") && !calledByForHeader))
            {
                flag = true;
            }

            check_scope = check_scope.GetParentScope();
        } while (check_scope != null && flag == false);
            
        if (!flag)
        {
            throw new SemanticException($"Variable is not defined \"{firstToken.GetSourceText()}\"");
        }
        
        Debug.Log($"Reference got op token {opToken.GetSourceText()}");
        
        if (opToken is In)
        {
            if (calledByForHeader)
            {
                lastToken = opToken;
                return;
            }
            
            throw new UnexpectedTokenException("Expected Reference op-token but got " + opToken);
        }

        if (opToken is LeftBracket)
        {
            whatReference = WhatReference.Array;
            var arrayAccess = NodeFactory.ConstructNode(new ArrayAccessNode(), lexer, opToken, out lastToken);
            children.Add(arrayAccess);
            arrayIndex = arrayAccess.GetArrayIndex();;
        }
        else if (opToken is LeftBrace)
        {
            whatReference = WhatReference.Call;
            children.Add(NodeFactory.ConstructNode(new FunctionCallNode(), lexer, opToken, out lastToken));
        }
        else if (opToken is Point)
        {
            whatReference = WhatReference.Tuple;
            var tupleNode = NodeFactory.ConstructNode(new TupleAccessNode(), lexer, opToken, out lastToken);
            children.Add(tupleNode);
            if (tupleNode.GetWhatTupleReference() is WhatTupleReference.TupleByIdent)
            {
                tupleAccessIdentifier = tupleNode.GetTupleAccessIdentifier();
            }
            else if (tupleNode.GetWhatTupleReference() is WhatTupleReference.TupleByIndex)
            {
                tupleIndexInt = tupleNode.GetTupleIndex();
            }
            whatTupleReference = tupleNode.GetWhatTupleReference();
        }
        else
        {
            whatReference = WhatReference.Ident;
            lastToken = opToken;
            Debug.Log($"Constructed simple ident-ref, returning {lastToken.GetSourceText()} as last token");
        }
    }
}
