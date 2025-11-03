using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class StringLiteral : TreeNode
{
    public override string GetName()
    {
        return "STRING" + " " + firstToken.GetSourceText();
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
