using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Types;
using String = compiler_construction.Tokenization.Types.String;

namespace compiler_construction.Syntax;

public class TypeIndicatorNode : TreeNode
{
    private string name;
    
    public override string GetName()
    {
        return "TypeIndicator";
    }

    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is Int)
        {
            name = "int";
        }
        else if (firstToken is Bool)
        {
            name = "bool";
        }
        else if (firstToken is Real)
        {
            name = "real";
        }
        else if (firstToken is String)
        {
            name = "string";
        }
        else if (firstToken is None)
        {
            name = "no type";
        }
        else if (firstToken is LeftBracket)
        {
            var token = lexer.GetNextToken();
            if (token is not RightBracket)
            {
                throw new UnexpectedTokenException($"Expected ], as part of type indicator, got {token}");
            }
            
            name = "vector type";
        }
        else if (firstToken is LeftCurlyBrace)
        {
            var token = lexer.GetNextToken();
            if (token is not RightCurlyBrace)
            {
                throw new UnexpectedTokenException("Expected }, as part of type indicator, got " + token);
            }

            name = "tuple";
        }
        else if (firstToken is Func)
        {
            name = "func";
        }
        else
        {
            throw new UnexpectedTokenException("Expected type indicator, got " + firstToken);
        }

        lastToken = lexer.GetNextToken();
    }
}
