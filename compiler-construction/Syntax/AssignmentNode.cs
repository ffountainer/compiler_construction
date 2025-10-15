using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class AssignmentNode : TreeNode
{
    public override string GetName()
    {
        return "Assignment";
    }

    public override void ReadTokens(out Token lastToken)
    {
        children.Add(NodeFactory.ConstructNode(new ReferenceNode(), lexer, firstToken, out var colonEqual));

        if (colonEqual is not ColonEqual)
        {
            throw new UnexpectedTokenException($"Expected :=, got {colonEqual}");
        }
        
        children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken()));
        lastToken = lexer.GetNextToken();
    }
}
