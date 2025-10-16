using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Syntax;

/// <summary>
/// Should place following token as last token
/// </summary>
public class UnaryNode : TreeNode
{
    private bool calledByForHeader;

    public UnaryNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Unary";
    }

    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is Plus || firstToken is Minus || firstToken is Not)
        {
            children.Add(NodeFactory.ConstructNode(new PrimaryNode(), lexer, lexer.GetNextToken(), out lastToken));
            return;
        }
        
        children.Add(NodeFactory.ConstructNode(new ReferenceNode(calledByForHeader), lexer, firstToken, out lastToken));
        
        if (lastToken is Is)
        {
            var token = lexer.GetNextToken();
            children.Add(NodeFactory.ConstructNode(new TypeIndicatorNode(), lexer, token, out lastToken));
        }
        
        Debug.Log($"Unary returning {lastToken.GetSourceText()} as last token");
    }
}
