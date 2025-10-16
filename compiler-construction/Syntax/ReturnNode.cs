using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class ReturnNode : TreeNode
{
    public override string GetName()
    {
        return "Return";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();

        // Since the expression is optional
        if (token is StatementSeparator)
        {
            lastToken = token;
            return;
        }
        
        children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out lastToken));
    }
}
