using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Syntax;

/// <summary>
/// Sets following token as last token
/// </summary>
public class ExpressionNode : TreeNode
{
    private bool calledByForHeader = false;

    public ExpressionNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Expression";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        Token token = firstToken;
        do
        {
            children.Add(NodeFactory.ConstructNode(new RelationNode(calledByForHeader), lexer, token, out lastToken));
        } while (AcceptableOperation(lastToken));
        
        Debug.Log($"Expression returning {lastToken.GetSourceText()} as last token");
    }

    private bool AcceptableOperation(Token token)
    {
        return token is Or || token is Xor || token is And;
    }
}
