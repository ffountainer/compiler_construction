using System.Collections;
using compiler_construction.Interpretation;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class FunBodyNode : TreeNode
{
    public override string GetName()
    {
        return "FunBody";
    }
    
    private WhatFunction WhatFunc;
    private List<StatementNode> Body;
    private ExpressionNode shortFuncExpr;
    
    public WhatFunction GetWhatFunc()
    {
        return WhatFunc;
    }
    public List<StatementNode> GetBody()
    {
        return Body;
    }
    public ExpressionNode GetShortFuncExpr()
    {
        return shortFuncExpr;
    }

    public override void ReadTokens(out Token lastToken)
    {
        Scope new_scope = new Scope(new Hashtable(), SyntaxAnalyzer.GetCurrentScope());
        SyntaxAnalyzer.SetScope(new_scope);
        Debug.Log($"Starting fun body from {firstToken}");
        
        if (firstToken is EqualGreater)
        {
            var node = NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out lastToken);
            children.Add(node);
            shortFuncExpr = node;
            WhatFunc = WhatFunction.Short;

        }
        else if (firstToken is Is)
        {
            var node = NodeFactory.ConstructNode(new BodyNode(), lexer, lexer.GetNextToken(), out lastToken);
            children.Add(node);
            Body = node.GetBody();
            WhatFunc = WhatFunction.Full;
            if (lastToken is not End)
            {
                throw new UnexpectedTokenException($"Expected end, got {lastToken}");
            }
            
            lastToken = lexer.GetNextToken();
        }
        else
        {
            throw new UnexpectedTokenException($"Expected => or is to start func definition but got {firstToken}");
        }
        SyntaxAnalyzer.SetScope(SyntaxAnalyzer.GetParentScope());
    }
}