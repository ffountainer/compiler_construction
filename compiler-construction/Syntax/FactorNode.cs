using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Syntax;

public class FactorNode : TreeNode
{
    private bool calledByForHeader;

    public FactorNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Factor";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        Token opToken;
        var token = firstToken;
        do
        {
            var node = new TermNode(calledByForHeader && firstToken == token);
            children.Add(NodeFactory.ConstructNode(node, lexer, token, out opToken));

            if (opToken is Plus || opToken is Minus)
            {
                token = lexer.GetNextToken();
            }
        } while (opToken is Plus || opToken is Minus);

        lastToken = opToken;
    }
}