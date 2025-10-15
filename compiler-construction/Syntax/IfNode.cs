using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class IfNode : TreeNode
{
    private bool isShort;
    
    public override string GetName()
    {
        return isShort ? "IfShort" : "If";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();
        children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out var thenOrArrow));

        if (thenOrArrow is Then)
        {
            isShort = false;
        }
        else if (thenOrArrow is EqualGreater)
        {
            isShort = true;
        }
        else
        {
            throw new UnexpectedTokenException($"Expected if statement body start, got {thenOrArrow}");
        }

        token = lexer.GetNextToken();
        children.Add(NodeFactory.ConstructNode(new BodyNode(), lexer, token, out var elseOrEnd));

        if (elseOrEnd is StatementSeparator)
        {
            lastToken = elseOrEnd;
            return;
        }

        if (elseOrEnd is Else && isShort)
        {
            throw new UnexpectedTokenException("No else allowed on short if.");
        }

        if (elseOrEnd is not StatementSeparator && elseOrEnd is not Else)
        {
            throw new UnexpectedTokenException($"Expected else or statement end. Got {elseOrEnd}");
        }
        
        token = lexer.GetNextToken();
        children.Add(NodeFactory.ConstructNode(new BodyNode(), lexer, token, out var end));

        if (end is not End)
        {
            throw new UnexpectedTokenException($"Expected end of if. Got {end}");
        }

        lastToken = end;
    }
}