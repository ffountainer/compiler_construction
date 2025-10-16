using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class ExitNode : TreeNode
{
    public override string GetName()
    {
        return "Exit";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = lexer.GetNextToken();
        Debug.Log($"Constructed exit node, lest token: {lastToken}");
    }
}
