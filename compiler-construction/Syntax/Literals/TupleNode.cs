using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax.Literals;

public class TupleNode : TreeNode
{
    public override string GetName()
    {
        return "Tuple";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();
        if (token is RightCurlyBrace)
        {
            throw new UnexpectedTokenException("Cannot init empty tuples");
        }

        Token opToken;
        do
        {
            Debug.Log($"TupleNode is adding new TupleElementNode, starting from {token.GetSourceText()}");
            children.Add(NodeFactory.ConstructNode(new TupleElementNode(), lexer, token, out opToken));
            Debug.Log($"TupleNode got opToken {opToken.GetSourceText()}");

            if (opToken is not Comma && opToken is not RightCurlyBrace)
            {
                var message = $"Expected comma or curly brace as part of tuple definition, got {opToken}";
                throw new UnexpectedTokenException(message);
            }
        } while (opToken is Comma);

        if (opToken is not RightCurlyBrace)
        {
            throw new UnexpectedTokenException($"Expected right curly brace after tuple definition, got {opToken}");
        }

        lastToken = lexer.GetNextToken();
    }
}
