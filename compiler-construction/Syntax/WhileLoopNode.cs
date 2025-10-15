namespace compiler_construction.Syntax;

public class WhileLoopNode : TreeNode
{
    public override string GetName()
    {
        return "WhileLoop";
    }

    public override void ReadTokens()
    {
        throw new NotImplementedException();
    }
}
