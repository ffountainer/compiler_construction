using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class FunctionCallNode : TreeNode
{
    public override string GetName()
    {
        return "FunctionCall";
    }

    public override void ReadTokens(out Token lastToken)
    {
        throw new NotImplementedException();
    }
}