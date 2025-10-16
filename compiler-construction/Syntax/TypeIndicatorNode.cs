using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class TypeIndicatorNode : TreeNode
{
    public override string GetName()
    {
        return "TypeIndicator";
    }

    public override void ReadTokens(out Token lastToken)
    {
        throw new NotImplementedException();
    }
}