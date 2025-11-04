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

    public override void ReadTokens(out Token lastToken)
    {
        IsFunc = true;
        
        var token = lexer.GetNextToken();
        if (token is LeftBrace)
        {
            do
            {
                token = lexer.GetNextToken();
                children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, token));
                token = lexer.GetNextToken();
            } while (token is Comma);

            if (token is not RightBrace)
            {
                throw new UnexpectedTokenException($"Expected ) to close func param block, got {token}");
            }
            
            token = lexer.GetNextToken();
        }
        
        children.Add(NodeFactory.ConstructNode(new FunBodyNode(), lexer, token, out lastToken));
        IsFunc = false;
    }
}