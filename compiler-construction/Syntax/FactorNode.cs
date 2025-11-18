using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;
using compiler_construction.Tokenization.Types;

namespace compiler_construction.Syntax;

public class FactorNode : ConstReduceableNode
{
    private bool calledByForHeader;
    private List<Token> operators = new List<Token>();
    private List<ConstReduceableNode> operands = new List<ConstReduceableNode>();

    public List<Token> GetOperators()
    {
        return operators;
    }

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
        IsConst = true;
        
        Token opToken;
        var token = firstToken;
        
        do
        {
            var node = new TermNode(calledByForHeader && firstToken == token); 
            node = NodeFactory.ConstructNode(node, lexer, token, out opToken);
            children.Add(node);
            operands.Add(node);

            if (!node.IsConst && IsConst)
            {
                IsConst = false;
            }

            if (opToken is Plus || opToken is Minus)
            {
                operators.Add(opToken);
                token = lexer.GetNextToken();
            }
        } while (opToken is Plus || opToken is Minus);

        lastToken = opToken;
        Debug.Log($"Factor returns {lastToken.GetSourceText()} as last token");
    }

    protected override void Calculate()
    {
        bool hasReal = false;
        bool hasBool = false;
        foreach (var operand in operands)
        {
            if (operand.GetValueType() == ConstValueType.Boolean)
            {
                hasBool = true;
            }
            else if (operand.GetValueType() == ConstValueType.Real)
            {
                hasReal = true;
            }
        }

        if (hasBool)
        {
            if (operators.Count > 0)
            {
                throw new SemanticException("Cannot add or subtract with booleans");
            }

            ValueType = ConstValueType.Boolean;
            BoolValue = operands.First().GetBoolValue();
            
            return;
        }
        
        ValueType = hasReal ? ConstValueType.Real : ConstValueType.Int;
        
        if (operands.First().GetValueType() == ConstValueType.Real) RealValue = operands.First().GetRealValue();
        else IntValue =  operands.First().GetIntValue();
        
        if (ValueType == ConstValueType.Real && ValueType != operands.First().GetValueType())
        {
            RealValue = IntValue;
        }

        for (int i = 0; i < operators.Count; i++)
        {
            var op = operators[i];
            var rightOperand = operands[i + 1];
            
            if (ValueType == ConstValueType.Real)
            {
                double rightOperandValue = rightOperand.GetValueType() == ConstValueType.Real
                    ? rightOperand.GetRealValue()
                    : rightOperand.GetIntValue();
                
                Debug.Log($"The left side is {RealValue} or int ({IntValue}) and the right side is {rightOperandValue}");
                
                RealValue = op is Plus
                    ? RealValue + rightOperandValue
                    // Else operator is Minus
                    : RealValue -  rightOperandValue;
            }
            else // Int
            {
                IntValue = op is Plus
                    ? IntValue + rightOperand.GetIntValue()
                    : IntValue - rightOperand.GetIntValue();
            }
        }
    }
}
