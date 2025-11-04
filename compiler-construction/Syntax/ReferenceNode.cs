using System.Collections;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class ReferenceNode : TreeNode
{
    private bool calledByForHeader;

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
            children.Add(NodeFactory.ConstructNode(new ArrayElementNode(), lexer, opToken, out lastToken));
        }
        else if (opToken is LeftBrace)
        {
            children.Add(NodeFactory.ConstructNode(new FunctionCallNode(), lexer, opToken, out lastToken));
        }
        else if (opToken is Point)
        {
            children.Add(NodeFactory.ConstructNode(new TupleAccessNode(), lexer, opToken, out lastToken));
        }
        else
        {
            children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, firstToken));
            lastToken = opToken;
            Debug.Log($"Constructed simple ident-ref, returning {lastToken.GetSourceText()} as last token");
        }
    }
}
