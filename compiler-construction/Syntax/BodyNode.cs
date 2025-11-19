using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Syntax;

public class BodyNode : TreeNode
{
    public override string GetName()
    {
        return "Body";
    }
    
    private List<StatementNode> Body = new List<StatementNode>();
    
    public List<StatementNode> GetBody()
    {
        return Body;
    }

    override public void ReadTokens(out Token lastToken)
    {
        lastToken = null;
        var token = firstToken;
        bool hasStatements = false;
        while (token is not End && token is not Else)
        {
            var node = NodeFactory.ConstructNode(new StatementNode(), lexer, token, out lastToken);
            children.Add(node);
            Body.Add(node);
            hasStatements = true;
            token = lastToken;
            
            Debug.Log($"Finished statement in body, got {token}");
        }

        if (!hasStatements)
        {
            throw new Exception("Empty body is not supported.");
        }

        lastToken = token;
        Debug.Log(">> Done constructing body");
    }
}
