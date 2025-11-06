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
            var node = NodeFactory.ConstructNode(new StatementNode(), lexer, token, out lastToken);
            
            if (!node.IsMeaningless())
            {
                children.Add(node);
            }
            
            token = lastToken;
        }
    }
}
