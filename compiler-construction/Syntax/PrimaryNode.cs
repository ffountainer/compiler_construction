using compiler_construction.Semantics;
using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Types;

namespace compiler_construction.Syntax;

/// <summary>
/// Should place following token as last token
/// </summary>
public class PrimaryNode : ConstReduceableNode
{
    private ExpressionNode? expressionNode;
    private LiteralNode? literalNode;
    
    public override string GetName()
    {
        return "Primary";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is LeftBrace)
        {
            var node = new ExpressionNode();
            var token = lexer.GetNextToken();
            expressionNode = NodeFactory.ConstructNode(node, lexer, token, out var closingBrace);
            children.Add(expressionNode);

            IsConst = node.IsConst;

            if (closingBrace is not RightBrace)
            {
                throw new UnexpectedTokenException($"Expected closing ) as part of Primary, got {closingBrace}");
            }

            lastToken = lexer.GetNextToken();
            return;
        }

        if (firstToken is Func)
        {
            children.Add(NodeFactory.ConstructNode(new FunctionLiteralNode(), lexer, firstToken, out lastToken));
            return;
        }

        literalNode = NodeFactory.ConstructNode(new LiteralNode(), lexer, firstToken, out lastToken);
        children.Add(literalNode);
        IsConst = true;
    }
    
    protected override void Calculate()
    {
        // TODO: Add calculation when the child node is an expression
        if (literalNode != null || expressionNode != null)
        {
            var node = literalNode ?? expressionNode as ConstReduceableNode;
            
            ValueType = node.GetValueType();
            if (node.GetValueType() == ConstValueType.Int)
            {
                IntValue = node.GetIntValue();
            }
            if (node.GetValueType() == ConstValueType.Real)
            {
                RealValue = node.GetRealValue();
            }
            if (node.GetValueType() == ConstValueType.Boolean)
            {
                BoolValue = node.GetBoolValue();
            }
        }
        else
        {
            throw new SemanticException("Primary: Got to calculating even though nodes are null");
        }
    }
}
