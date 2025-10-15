using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class WhileLoopNode : TreeNode
{
    public override string GetName()
    {
        return "WhileLoop";
    }

    public override void ReadTokens(out Token lastToken)
    {
        // зондре дабалатория
        var token = lexer.GetNextToken();
        children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out var bodyStart));
        children.Add(NodeFactory.ConstructNode(new LoopBodyNode(), lexer, bodyStart, out lastToken));
    }
}
