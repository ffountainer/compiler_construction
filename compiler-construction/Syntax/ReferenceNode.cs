namespace compiler_construction.Syntax;

public class ReferenceNode : TreeNode
{
    public override string GetName()
    {
        return "Reference";
    }

    public override void ReadTokens()
    {
        throw new NotImplementedException();
    }
}