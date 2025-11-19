using System.Collections;
using compiler_construction.Interpretation;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

/// <summary>
/// Following token set as last token
/// </summary>
public class FunctionLiteralNode : TreeNode
{
    public override string GetName()
    {
        return "FunctionLiteral";
    }
    
    private List<String> arguments;
    private WhatFunction WhatFunc;
    private List<StatementNode> Body;
    private ExpressionNode shortFuncExpr;

    public FunctionLiteralNode WithValues(List<String> args, WhatFunction whatFunc, List<StatementNode> body,
        ExpressionNode shortFuncEx)
    {
        arguments = args;
        WhatFunc = whatFunc;
        Body = body;
        shortFuncExpr = shortFuncEx;
        return this;
    }

    public ExpressionNode GetShortFuncExpr()
    {
        return shortFuncExpr;
    }

    public List<StatementNode> GetBody()
    {
        return Body;
    }
    
    public List<String> GetArguments()
    {
        return arguments;
    }

    public WhatFunction GetWhatFunc()
    {
        return WhatFunc;
    }

    public override void ReadTokens(out Token lastToken)
    {
        IsFunc = true;
        Scope new_scope = new Scope(new Hashtable(), SyntaxAnalyzer.GetCurrentScope());
        SyntaxAnalyzer.SetScope(new_scope);
        var token = lexer.GetNextToken();
        if (token is LeftBrace)
        {
            do
            {
                token = lexer.GetNextToken();
                SyntaxAnalyzer.AddToCurScope(token.GetSourceText(), true);
                var node = NodeFactory.ConstructNode(new IdentifierNode(), lexer, token);
                children.Add(node);
                arguments.Add(node.GetValue());
                token = lexer.GetNextToken();
            } while (token is Comma);

            if (token is not RightBrace)
            {
                throw new UnexpectedTokenException($"Expected ) to close func param block, got {token}");
            }
            
            token = lexer.GetNextToken();
        }

        var funBody = NodeFactory.ConstructNode(new FunBodyNode(), lexer, token, out lastToken);
        children.Add(funBody);
        WhatFunc = funBody.GetWhatFunc();
        if (WhatFunc is WhatFunction.Full)
        {
            Body = funBody.GetBody();
        }

        if (WhatFunc is WhatFunction.Short)
        {
            shortFuncExpr = funBody.GetShortFuncExpr();
        }
        SyntaxAnalyzer.SetScope(SyntaxAnalyzer.GetCurrentScope().GetParentScope());
        IsFunc = false;
    }
}