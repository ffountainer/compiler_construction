using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class BooleanNode : TreeNode
{
    public override string GetName()
    {
        return "Boolean";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
