using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class BooleanLiteral : TreeNode
{
    public override string GetName()
    {
        return "Boolean" + " " + firstToken.GetSourceText();
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
