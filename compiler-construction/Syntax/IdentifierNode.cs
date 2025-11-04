using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class IdentifierNode : TreeNode
{
    public override string GetName()
    {
        return "Identifier" + " " + firstToken.GetSourceText();
    }

    public override void ReadTokens(out Token lastToken)
    {
        Debug.Log($"I got {firstToken.GetSourceText()}");
        lastToken = firstToken;
    }
}
