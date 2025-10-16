using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class IntegerNode : TreeNode
{
    public override string GetName()
    {
        return "INTEGER";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
