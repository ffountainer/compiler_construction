using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Syntax;

public class RelationNode : TreeNode
{
    private bool calledByForHeader;

    public RelationNode(bool calledByForHeader)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Relation";
    }

    public override void ReadTokens(out Token lastToken)
    {
        children.Add(NodeFactory.ConstructNode(new FactorNode(calledByForHeader), lexer, firstToken, out var opToken));

        if (AcceptableOperation(opToken))
        {
            children.Add(NodeFactory.ConstructNode(new FactorNode(), lexer, lexer.GetNextToken(), out lastToken));
        }
        else
        {
            lastToken = opToken;
        }
        
        Debug.Log($"Relation node returns {lastToken.GetSourceText()} as last token");
    }

    private bool AcceptableOperation(Token token)
    {
        return token is Greater || token is GreaterEqual
            || token is Less || token is LessEqual
            || token is Equal || token is NotEqual;
    }
}