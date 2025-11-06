using compiler_construction.Semantics;
using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;
using compiler_construction.Tokenization.Types;

namespace compiler_construction.Syntax;

public class TermNode : ConstReduceableNode
{
    private bool calledByForHeader;
    private List<Token> operators = [];
    private List<ConstReduceableNode> operands = [];

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
        IsConst = true;
        Token token = firstToken;
        do
        {
            var node = new UnaryNode(calledByForHeader && token == firstToken);
            node = NodeFactory.ConstructNode(node, lexer, token, out lastToken);
            children.Add(node);
            operands.Add(node);

            if (!node.IsConst && IsConst)
            {
                IsConst = false;
            }

            if (lastToken is Times || lastToken is Divide)
            {
                operators.Add(lastToken);
                token = lexer.GetNextToken();
            }
        } while (lastToken is Times || lastToken is Divide);
        
        Debug.Log($"Term returning {lastToken.GetSourceText()} as last token");
    }

    protected override void Calculate()
    {
        bool hasReal = false, hasBool = false, hasInt = false;
        foreach (var operand in operands) 
        {
            // Since we are here, all nodes are considered const during readTokens
            if (operand.GetValueType() == ConstValueType.Boolean) hasBool = true;
            if (operand.GetValueType() == ConstValueType.Real) hasReal = true;
            if (operand.GetValueType() == ConstValueType.Int) hasInt = true;
        }

        if (hasBool)
        {
            if (operators.Count > 0)
            {
                throw new SemanticException("Cannot apply term operators to bool operands");
            }

            ValueType = ConstValueType.Boolean;
            BoolValue = operands.First().GetBoolValue();

            return;
        }
        
        ValueType = hasReal ? ConstValueType.Real : ConstValueType.Int;
        
        if (ValueType == ConstValueType.Real) RealValue =  operands.First().GetRealValue();
        else IntValue = operands.First().GetIntValue();

        for (int i = 0; i < operators.Count; i++)
        {
            var op =  operators[i];
            var rightOperand = operands[i + 1];

            if (ValueType == ConstValueType.Real)
            {
                double rightOperandValue = rightOperand.GetValueType() == ConstValueType.Int
                    ? rightOperand.GetIntValue()
                    : rightOperand.GetRealValue();

                RealValue = op is Divide
                    ? RealValue / rightOperandValue
                    // Else operand is Times
                    : RealValue *  rightOperandValue;
            }
            else // Int
            {
                IntValue = op is Divide
                    ? IntValue / rightOperand.GetIntValue()
                    : IntValue * rightOperand.GetIntValue();
            }
        }
    }
}
