using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Syntax;

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
        Token opToken;
        Token token = firstToken;
        do
        {
            children.Add(NodeFactory.ConstructNode(new RelationNode(calledByForHeader), lexer, token));

            opToken = lexer.GetNextToken();
            if (!AcceptableOperation(opToken))
            {
                throw new UnexpectedTokenException($"Unexpected token for connecting relations: {opToken}");
            }
        } while (AcceptableOperation(opToken));

        lastToken = opToken;
    }

    private bool AcceptableOperation(Token token)
    {
        return token is Or || token is Xor || token is And;
    }
}
