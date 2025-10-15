using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class ForLoopNode : TreeNode
{
    public override string GetName()
    {
        return "ForLoop";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        throw new NotImplementedException();
    }
}