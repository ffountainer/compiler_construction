using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class StringNode : TreeNode
{
    public override string GetName()
    {
        return "STRING";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
