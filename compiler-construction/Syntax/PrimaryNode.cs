using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Types;

namespace compiler_construction.Syntax;

/// <summary>
/// Should place following token as last token
/// </summary>
public class PrimaryNode : TreeNode
{
    public override string GetName()
    {
        return "Primary";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is LeftBrace)
        {
            var node = new ExpressionNode();
            var token = lexer.GetNextToken();
            children.Add(NodeFactory.ConstructNode(node, lexer, token, out var closingBrace));

            if (closingBrace is not RightBrace)
            {
                throw new UnexpectedTokenException($"Expected closing ) as part of Primary, got {closingBrace}");
            }

            lastToken = lexer.GetNextToken();
            return;
        }

        if (firstToken is Func)
        {
            children.Add(NodeFactory.ConstructNode(new FunctionLiteralNode(), lexer, firstToken, out lastToken));
            return;
        }
        
        children.Add(NodeFactory.ConstructNode(new LiteralNode(), lexer, firstToken));
        lastToken = lexer.GetNextToken();
    }
}