using compiler_construction.Interpretation;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Syntax;

/// <summary>
/// Sets following token as last token
/// </summary>
public class ExpressionNode : ConstReduceableNode
{
    private bool calledByForHeader = false;

    private List<ConstReduceableNode> operands = [];
    private List<Token> operators = [];

    public List<ConstReduceableNode> GetOperands()
    {
        return operands;
    }

    public WhatExpression WhatExpression;

    public List<Token> GetOperators()
    {
        return operators;
    }
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
        IsConst = true;
        Token token = firstToken;
        do
        {
            var node = NodeFactory.ConstructNode(new RelationNode(calledByForHeader), lexer, token, out lastToken);
            children.Add(node);
            operands.Add(node);

            if (!node.IsConst && IsConst)
            {
                IsConst = false;
            }
            
            if (AcceptableOperation(lastToken))
            {
                operators.Add(lastToken);
                token = lexer.GetNextToken();
            }
            
        } while (AcceptableOperation(lastToken));
        
        Debug.Log($"Expression returning {lastToken.GetSourceText()} as last token");
    }

    private bool AcceptableOperation(Token token)
    {
        return token is Or || token is Xor || token is And;
    }

    protected override void Calculate()
    {
        if (operators.Count == 0)
        {
            var child = operands.First();
            ValueType = child.GetValueType();

            if (ValueType == ConstValueType.Boolean) BoolValue = child.GetBoolValue();
            else if (ValueType == ConstValueType.Int) IntValue = child.GetIntValue();
            else RealValue =  child.GetRealValue();

            return;
        }

        ValueType = ConstValueType.Boolean;
        
        foreach (var operand in operands)
        {
            if (operand.GetValueType() != ConstValueType.Boolean)
            {
                throw new SemanticException("Cannot apply Expression operators to non-boolean values");
            }
        }

        // Operator priority: Xor > And > Or
        // https://stackoverflow.com/a/12494812

        List<bool> operandValues = [];
        foreach (var operand in operands)
        {
            operandValues.Add(operand.GetBoolValue());
        }
        
        var opIndex = GetOperatorIndex(typeof(Xor));
        while (opIndex != -1)
        {
            operandValues[opIndex] = operandValues[opIndex] ^ operandValues[opIndex + 1];
            operandValues.RemoveAt(opIndex + 1);
            operators.RemoveAt(opIndex);

            opIndex = GetOperatorIndex(typeof(Xor));
        }

        opIndex = GetOperatorIndex(typeof(And));
        while (opIndex != -1)
        {
            operandValues[opIndex] = operandValues[opIndex] && operandValues[opIndex + 1];
            operandValues.RemoveAt(opIndex + 1);
            operators.RemoveAt(opIndex);
            
            opIndex = GetOperatorIndex(typeof(And));
        }
        
        opIndex =  GetOperatorIndex(typeof(Or));
        while (opIndex != -1)
        {
            operandValues[opIndex] = operandValues[opIndex] || operandValues[opIndex + 1];
            operandValues.RemoveAt(opIndex + 1);
            operators.RemoveAt(opIndex);
            
            opIndex = GetOperatorIndex(typeof(Or));
        }

        if (operators.Count > 0)
        {
            // Should never throw
            throw new SemanticException($"{operators.Count} operators left after reduction...");
        }
        
        BoolValue = operandValues[0];
    }

    private int GetOperatorIndex(Type type)
    {
        for (int i = 0; i < operators.Count; i++)
        {
            if (operators[i].GetType() == type)
            {
                return i;
            }
        }
        
        return -1;
    }
}
