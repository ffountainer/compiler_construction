using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class VariableDefinitionNode : TreeNode
{
    public override string GetName()
    {
        return "VariableDefinition";
    }

    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is not Identifier)
        {
            throw new UnexpectedTokenException("Expected identifier, but got " + firstToken);
        }
        
        children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, firstToken));
        
        lastToken = lexer.GetNextToken();
        if (lastToken is ColonEqual)
        {
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out lastToken));
        }
    }
}