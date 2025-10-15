using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class ExpressionNode : TreeNode
{
    public override string GetName()
    {
        return "Expression";
    }

    public override void ReadTokens(out Token lastToken)
    {
        throw new NotImplementedException();
    }
}
