using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class ProgramNode : TreeNode
{
    public override string GetName() => "Program";

    override public void ReadTokens(out Token lastToken)
    {
        lastToken = null;
        var token = firstToken;
        while (lastToken is not FinishProgram)
        {
            children.Add(NodeFactory.ConstructNode(new StatementNode(), lexer, token, out lastToken));
            token = lastToken;
        }
    }
}
