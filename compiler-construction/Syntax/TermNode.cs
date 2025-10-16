using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Syntax;

public class TermNode : TreeNode
{
    private bool calledByForHeader;

    public TermNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Term";
    }

    public override void ReadTokens(out Token lastToken)
    {
        Token token = firstToken;
        Token opToken;
        do
        {
            var node = new UnaryNode(calledByForHeader && token == firstToken);
            children.Add(NodeFactory.ConstructNode(node, lexer, token,  out lastToken));

            if (lastToken is Times || lastToken is Divide)
            {
                token = lexer.GetNextToken();
            }
        } while (lastToken is Times || lastToken is Divide);
        
        Debug.Log($"Term returning {lastToken.GetSourceText()} as last token");
    }
}