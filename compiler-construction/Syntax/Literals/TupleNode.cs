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
            children.Add(NodeFactory.ConstructNode(new TupleElementNode(), lexer, token, out opToken));

            if (opToken is not Comma && opToken is not RightCurlyBrace)
            {
                throw new UnexpectedTokenException("Expected comma or curly brace as part of tuple definition");
            }
        } while (opToken is Comma);

        lastToken = opToken;
    }
}