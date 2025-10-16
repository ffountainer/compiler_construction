using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax.Literals;

/// <summary>
/// Puts comma or closing bracket as last token
/// </summary>
public class TupleElementNode : TreeNode
{
    public override string GetName()
    {
        return "TupleElement";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = firstToken;
        if (token is Identifier)
        {
            children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, token, out lastToken));

            if (lastToken is not ColonEqual)
            {
                throw new UnexpectedTokenException($"Expected := as part of tuple element definition, got {lastToken}");
            }

            token = lexer.GetNextToken();
        }
        
        children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out lastToken));
    }
}
