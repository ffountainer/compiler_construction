namespace compiler_construction.Syntax;

public class AssignmentNode : TreeNode
{
    public override string GetName()
    {
        return "Assignment";
    }

    public override void ReadTokens()
    {
        throw new NotImplementedException();
    }
}
