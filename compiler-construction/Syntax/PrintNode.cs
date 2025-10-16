using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class PrintNode : TreeNode
{
    public override string GetName()
    {
        return "Print";
    }

    public override void ReadTokens(out Token lastToken)
    {
        Token commaOrEnd;
        do
        {
            var token = lexer.GetNextToken();
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out commaOrEnd));

            if (commaOrEnd is not Comma && commaOrEnd is not StatementSeparator)
            {
                throw new UnexpectedTokenException($"Expected comma or statement end in print, got {commaOrEnd}");
            }
        } while (commaOrEnd is Comma);
        
        lastToken = commaOrEnd;
    }
}
