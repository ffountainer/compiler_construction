using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class LoopBodyNode : TreeNode
{
    public override string GetName()
    {
        return  "LoopBody";
    }

    public override void ReadTokens(out Token lastToken)
    {
        throw new NotImplementedException();
    }
}
