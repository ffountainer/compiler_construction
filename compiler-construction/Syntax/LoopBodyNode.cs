namespace compiler_construction.Syntax;

public class LoopBodyNode : TreeNode
{
    public override string GetName()
    {
        return  "LoopBody";
    }

    public override void ReadTokens()
    {
        throw new NotImplementedException();
    }
}
