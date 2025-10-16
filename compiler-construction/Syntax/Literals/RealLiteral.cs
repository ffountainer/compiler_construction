using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class RealLiteral : TreeNode
{
    public override string GetName()
    {
        return "REAL";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
