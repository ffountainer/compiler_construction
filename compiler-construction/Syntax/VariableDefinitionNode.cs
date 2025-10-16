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
            Debug.Log("Got colon equal in var def, construct expr");
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out lastToken));
            Debug.Log($"Var def got {lastToken.GetSourceText()} as last token out of expression");
        }
    }
}