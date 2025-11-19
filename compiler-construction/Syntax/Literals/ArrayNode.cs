using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax.Literals;

public class ArrayNode : TreeNode
{
    public List<ExpressionNode> Value = new List<ExpressionNode>();
    public override string GetName()
    {
        return "Array";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();
        
        // Check for empty array definition
        if (token is RightBracket)
        {
            lastToken = token;
            return;
        }
        
        while (true)
        {
            var node = NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out token);
            children.Add(node);
            Value.Add(node);
            if (token is Comma)
            {
                token = lexer.GetNextToken();
                continue;
            }

            if (token is RightBracket)
            {
                lastToken = token;
                return;
            }
            
            // Else
            throw new UnexpectedTokenException($"Expected comma or closing ] as part of array definition, got {token}");
        }
    }
    
    public ArrayNode WithValue(List<ExpressionNode> value)
    {
        Value = value;
        return this;
    }
}
