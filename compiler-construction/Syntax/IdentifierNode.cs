using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class IdentifierNode : TreeNode
{
    private String Value;

    public String GetValue()
    {
        return Value;
    }

    public override string GetName()
    {
        return "Identifier" + " " + firstToken.GetSourceText();
    }

    public override void ReadTokens(out Token lastToken)
    {
        Debug.Log($"I got {firstToken.GetSourceText()}");
        lastToken = firstToken;
        Value = firstToken.GetSourceText();
    }
}
