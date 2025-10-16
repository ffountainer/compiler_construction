using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class DeclarationNode : TreeNode
{
    public override string GetName()
    {
        return "Declaration";
    }

    public override void ReadTokens(out Token lastToken)
    {
        Token commaOrEnd;
        do
        {
            var node = new VariableDefinitionNode();
            var token = lexer.GetNextToken();
            Debug.Log($"Var def starts from {token.GetSourceText()}");
            children.Add(NodeFactory.ConstructNode(node, lexer, token, out commaOrEnd));

            if (commaOrEnd is not Comma && commaOrEnd is not StatementSeparator)
            {
                throw new UnexpectedTokenException($"Expected comma or statement end, got {commaOrEnd}");
            }
        } while (commaOrEnd is not StatementSeparator);

        lastToken = commaOrEnd;
    }
}
