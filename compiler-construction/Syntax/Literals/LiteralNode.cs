using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Types;
using String = compiler_construction.Tokenization.Types.String;

namespace compiler_construction.Syntax.Literals;

/// <summary>
/// Places following token as last token
/// </summary>
public class LiteralNode : TreeNode
{
    public override string GetName()
    {
        return "Literal";
    }

    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is Int)
        {
            children.Add(NodeFactory.ConstructNode(new IntegerLiteral(), lexer, firstToken, out lastToken));
        }
        else if (firstToken is Real)
        {
            children.Add(NodeFactory.ConstructNode(new RealLiteral(), lexer, firstToken, out lastToken));
        }
        else if (firstToken is String)
        {
            children.Add(NodeFactory.ConstructNode(new StringLiteral(), lexer, firstToken, out lastToken));
        }
        else if (firstToken is Bool)
        {
            children.Add(NodeFactory.ConstructNode(new BooleanLiteral(), lexer, firstToken, out lastToken));
        }
        else if (firstToken is LeftCurlyBrace)
        {
            children.Add(NodeFactory.ConstructNode(new TupleNode(), lexer, firstToken, out lastToken));
        }
        else if (firstToken is LeftBracket)
        {
            children.Add(NodeFactory.ConstructNode(new ArrayNode(), lexer, firstToken,  out lastToken));
        }
        else if (firstToken is None)
        {
            children.Add(NodeFactory.ConstructNode(new NoneLiteral(), lexer, firstToken, out lastToken));
        }
        else
        {
            throw new UnauthorizedAccessException($"Unexpected token for literal: {firstToken}");
        }

        lastToken = lexer.GetNextToken();
    }
}