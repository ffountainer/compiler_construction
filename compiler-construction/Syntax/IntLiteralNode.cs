using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class IntLiteralNode : TreeNode
{
    public override string GetName()
    {
        return "IntLiteral";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
