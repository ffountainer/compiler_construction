using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class NoneLiteral : TreeNode
{
    public override string GetName()
    {
        return "none";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
