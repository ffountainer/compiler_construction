using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class IdentifierNode : TreeNode
{
    public override string GetName()
    {
        return "Identifier";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
