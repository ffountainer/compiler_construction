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
        IsLoop = true;
        children.Add(NodeFactory.ConstructNode(new ForHeader(), lexer, firstToken, out var token));
        children.Add(NodeFactory.ConstructNode(new LoopBodyNode(), lexer, token, out lastToken));
        IsLoop = false;
    }
}
