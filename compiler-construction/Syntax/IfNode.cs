using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class IfNode : TreeNode
{
    private bool isShort;
    private bool everExecutes = true;
    private ExpressionNode expressionNode;
    
    public override string GetName()
    {
        return isShort ? "IfShort" : "If";
    }

    public bool GetEverExecutes() =>  everExecutes;
    
    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();

        expressionNode = new ExpressionNode();
        expressionNode = NodeFactory.ConstructNode(expressionNode, lexer, token, out var thenOrArrow);
        
        children.Add(expressionNode);
        
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
        
        // Short if is handled like
        // if expression => expression
        bool isShortEnd = false;
        if (isShort)
        {
            children.Add(NodeFactory.ConstructNode(new BodyNode(), lexer, token, out var isElse));
            
            CheckIfRedundant();
            
            if (isElse is End)
            {
                isShortEnd = true;
            }
            
        }

        if (isShort && isShortEnd)
        {
            lastToken = lexer.GetNextToken();
            CheckIfRedundant();
            return;
        }
        
        children.Add(NodeFactory.ConstructNode(new BodyNode(), lexer, token, out var elseOrEnd));

        if (elseOrEnd is not Else && elseOrEnd is not End)
        {
            throw new UnexpectedTokenException($"Expected else or end for if. Got {elseOrEnd}");
        }
        
        if (elseOrEnd is Else && isShort)
        {
            throw new UnexpectedTokenException("No else allowed on short if.");
        }

        if (elseOrEnd is End)
        {
            lastToken = lexer.GetNextToken();
            CheckIfRedundant();
            return;
        }
        
        // Construct body for else
        token = lexer.GetNextToken();
        children.Add(NodeFactory.ConstructNode(new BodyNode(), lexer, token, out var end));

        if (end is not End)
        {
            throw new UnexpectedTokenException($"Expected end of if. Got {end}");
        }

        lastToken = lexer.GetNextToken();
    }

    private void CheckIfRedundant()
    {
        if (expressionNode.IsConst)
        {
            if (expressionNode.GetValueType() == ConstValueType.Boolean && !expressionNode.GetBoolValue()
                || expressionNode.GetValueType() != ConstValueType.Boolean && expressionNode.GetNumericalValue() == 0)
            {
                everExecutes = false;
            }
        }
    }
}
