using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class FunctionCallNode : TreeNode
{
    public override string GetName()
    {
        return "FunctionCall";
    }

    public override void ReadTokens(out Token lastToken)
    {
        do
        {
            var token =  lexer.GetNextToken();
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out lastToken));
        } while (lastToken is Comma);

        if (lastToken is not RightBrace)
        {
            throw new UnexpectedTokenException($"Expected closing brace for func call, got {lastToken}");
        }
        
        lastToken = lexer.GetNextToken();
    }
}