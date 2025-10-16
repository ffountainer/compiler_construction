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
        if (firstToken is not Identifier || IsPrimaryOperator(firstToken))
        {
            Debug.Log($"Unary goes to construct Primary because [ ident: {firstToken is Identifier}, primary op: {IsPrimaryOperator(firstToken)} ], where firstToken is {firstToken.GetSourceText()}");
            
            var token = firstToken;
            if (IsPrimaryOperator(token))
            {
                token = lexer.GetNextToken();
            }
            
            Debug.Log($"Starting to construct primary from {token.GetSourceText()}");
            children.Add(NodeFactory.ConstructNode(new PrimaryNode(), lexer, token, out lastToken));
            Debug.Log($"Constructed primary in Unary, last token: {lastToken.GetSourceText()}");
            return;
        }
        
        children.Add(NodeFactory.ConstructNode(new ReferenceNode(calledByForHeader), lexer, firstToken, out lastToken));
        Debug.Log($"Unary finished constructing ref, last token: {lastToken.GetSourceText()}, constructed ref is {children.Last()}");
        
        if (lastToken is Is)
        {
            var token = lexer.GetNextToken();
            children.Add(NodeFactory.ConstructNode(new TypeIndicatorNode(), lexer, token, out lastToken));
        }
        
        Debug.Log($"Unary returning {lastToken.GetSourceText()} as last token");
    }

    private bool IsPrimaryOperator(Token token)
    {
        return token is Plus || token is Minus || token is Not;
    }
}
