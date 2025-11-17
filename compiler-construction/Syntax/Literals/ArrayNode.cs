using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax.Literals;

public class ArrayNode : TreeNode
{
    
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
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out token));

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
}
