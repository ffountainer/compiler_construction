namespace compiler_construction.Syntax;

public class ForLoopNode : TreeNode
{
    public override string GetName()
    {
        return "ForLoop";
    }

    public override void ReadTokens()
    {
        throw new NotImplementedException();
    }
}