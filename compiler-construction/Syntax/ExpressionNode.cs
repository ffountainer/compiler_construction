using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class ExpressionNode : TreeNode
{
    private bool calledByForHeader = false;

    public ExpressionNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Expression";
    }

    public override void ReadTokens(out Token lastToken)
    {
        throw new NotImplementedException();
    }
}
