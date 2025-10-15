using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Syntax;

public class StatementNode : TreeNode
{
    public override string GetName()
    {
        return "Statement";
    }

    public override void ReadTokens()
    {
        if (firstToken is Var)
        {
            children.Add(NodeFactory.ConstructNode(new VariableDeclarationNode(), lexer, firstToken));
        }
        else if (firstToken is If)
        {
            children.Add(NodeFactory.ConstructNode(new IfNode(), lexer, firstToken));
        }
        else if (firstToken is While)
        {
            children.Add(NodeFactory.ConstructNode(new WhileLoopNode(), lexer, firstToken));
        }
        else if (firstToken is For)
        {
            children.Add(NodeFactory.ConstructNode(new ForLoopNode(), lexer, firstToken));
        }
        else if (firstToken is Loop)
        {
            children.Add(NodeFactory.ConstructNode(new LoopBodyNode(),  lexer, firstToken));
        }
        else if (firstToken is Exit)
        {
            children.Add(NodeFactory.ConstructNode(new ExitNode(), lexer, firstToken));
        }
        else if (firstToken is Identifier)
        {
            children.Add(NodeFactory.ConstructNode(new AssignmentNode(), lexer, firstToken));
        }
        else
        {
            throw new UnexpectedTokenException($"Unexpected token {firstToken}");
        }
    }
}
