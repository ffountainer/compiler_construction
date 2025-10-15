using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using Range = compiler_construction.Tokenization.Symbols.Range;

namespace compiler_construction.Syntax;

public class ForHeader : TreeNode
{
    public override string GetName()
    {
        return "ForHeader";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();
        
        // Try to build an expression
        var node = NodeFactory.ConstructNode(new ExpressionNode(true), lexer, token, out var inToken);

        // If found the in keyword, then store ident first
        if (inToken is In)
        {
            children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, token));
            
            // Construct the expression that follows and store it
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out lastToken));
        }
        else
        {
            // If did not meet the in keyword, store the constructed expression
            children.Add(node);
            // Since we ignored last token here before
            lastToken = inToken;
        }

        if (lastToken is Range)
        {
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(),  lexer, lexer.GetNextToken(), out lastToken));
        }
    }
}