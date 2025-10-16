using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Syntax;

public class BodyNode : TreeNode
{
    public override string GetName()
    {
        return "Body";
    }

    override public void ReadTokens(out Token lastToken)
    {
        lastToken = null;
        var token = firstToken;
        bool hasStatements = false;
        while (token is not End)
        {
            children.Add(NodeFactory.ConstructNode(new StatementNode(), lexer, token, out lastToken));
            hasStatements = true;
            token = lastToken;
        }

        if (!hasStatements)
        {
            throw new Exception("Empty body is not supported.");
        }

        lastToken = token;
    }
}
