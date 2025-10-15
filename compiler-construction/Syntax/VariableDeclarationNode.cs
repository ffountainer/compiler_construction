using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class VariableDeclarationNode : TreeNode
{
    public override string GetName()
    {
        return "VariableDefinition";
    }

    public override void ReadTokens(out Token lastToken)
    {
        throw new System.NotImplementedException();
    }
}