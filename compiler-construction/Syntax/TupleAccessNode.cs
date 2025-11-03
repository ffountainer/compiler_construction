using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Types;

namespace compiler_construction.Syntax;

public class TupleAccessNode : TreeNode
{
    public override string GetName()
    {
        return "TupleAccess";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();
        if (token is Identifier)
        {
            children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, token));
        }
        else if (token is Int)
        {
            children.Add(NodeFactory.ConstructNode(new IntegerLiteral(), lexer, token));
        }
        else
        {
            throw new UnexpectedTokenException($"For tuple access expected int literal or identifier, got {token}");
        }
        
        lastToken = lexer.GetNextToken();
    }
}
