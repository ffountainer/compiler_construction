using compiler_construction.Syntax;

namespace compiler_construction;

public class SemanticAnalyser
{
    private ProgramNode InitialTree;

    private ProgramNode NewTree;

    public SemanticAnalyser(ProgramNode initial)
    {
        InitialTree = initial;
        NewTree = initial;
        NewTree.ClearChildren();
        SimplifyConstants(InitialTree,  NewTree);
    }

    public void PrintOptimisedTree()
    {
        NewTree.PrintTree();
    }

    public void SimplifyConstants(TreeNode root, TreeNode newRoot)
    {
        TreeNode currentNode = root;
        TreeNode newCurrentNode = newRoot;
        foreach (var child in currentNode.GetChildren()) 
        {
            if (child is ExpressionNode)
            {
                // trying to simplify expression
                List<RelationNode> relations = new List<RelationNode>();
                foreach (RelationNode relation in child.GetChildren())
                {
                    relations.Add(relation);
                }
            } 
            
            foreach (var chChild in child.GetChildren())
            {
                currentNode = chChild;
                SimplifyConstants(currentNode, newCurrentNode);
            }
                
        }

    }

}