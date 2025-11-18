using compiler_construction.Syntax;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Interpretation;

public class TermInterpreter : Interpretable
{
    private TermNode _term;

    public TermInterpreter(TermNode term)
    {
        _term = term;
        children = term.GetChildren();
    }
    
    public override void Interpret()
    {
        Debug.Log("Started to interpret term");
        List<TreeNode> operands = _term.GetChildren();
        List<UnaryInterpreter> unaryInterpreters = new List<UnaryInterpreter>(); 
        List<Token> operators = _term.GetOperators();
        foreach (UnaryNode node in operands)
        {
            UnaryInterpreter unaryInterpreter = new UnaryInterpreter(node);
            unaryInterpreter.Interpret();
            
            unaryInterpreters.Add(unaryInterpreter);
            if (unaryInterpreter.GetWhatExpression() != WhatExpression.RealExpr &&
                unaryInterpreter.GetWhatExpression() != WhatExpression.IntegerExpr &&
                unaryInterpreter.GetWhatExpression() != WhatExpression.BoolExpr &&
                unaryInterpreters.Count != 1)
            {
                throw new InterpretationException(
                    $"Interpretation: cannot apply * or / to {unaryInterpreter.GetWhatExpression()}");
            }
        }
        if (unaryInterpreters.Count == 1)
        {
            InheritValues(unaryInterpreters.First(), "Interpretation: error interpreting term while inheriting from a single unary");
        }
        
        else
        {
            bool hasReal = false, hasBool = false, hasInt = false;
            foreach (var unaryInterpreter in unaryInterpreters)
            {
                if (unaryInterpreter.GetWhatExpression() is WhatExpression.RealExpr) hasReal = true;
                if (unaryInterpreter.GetWhatExpression() is WhatExpression.BoolExpr) hasBool = true;
                if (unaryInterpreter.GetWhatExpression() is WhatExpression.IntegerExpr) hasInt = true;
            }

            if (hasBool)
            {
                if (operators.Count > 0)
                {
                    throw new InterpretationException("Cannot apply term operators to bool operands");
                }

                WhatExpr = WhatExpression.BoolExpr;
                BoolValue = unaryInterpreters.First().GetBoolValue();
                return;
            }

            WhatExpr = hasReal ? WhatExpression.RealExpr : WhatExpression.IntegerExpr;

            if (unaryInterpreters.First().GetWhatExpression() is WhatExpression.RealExpr)
                RealValue = unaryInterpreters.First().GetRealValue();
            else IntValue = unaryInterpreters.First().GetIntValue();

            if (WhatExpr is WhatExpression.RealExpr && WhatExpr != unaryInterpreters.First().GetWhatExpression())
            {
                RealValue = IntValue;
            }

            for (int i = 0; i < operators.Count; i++)
            {
                var op = operators[i];
                var rightOperand = unaryInterpreters[i + 1];

                if (WhatExpr == WhatExpression.RealExpr)
                {
                    double rightOperandValue = rightOperand.GetWhatExpression() == WhatExpression.IntegerExpr
                        ? rightOperand.GetIntValue()
                        : rightOperand.GetRealValue();
                    RealValue = op is Divide
                        ? RealValue / rightOperandValue
                        : RealValue * rightOperandValue;
                }
                else
                {
                    IntValue = op is Divide
                        ? IntValue / rightOperand.GetIntValue()
                        : IntValue * rightOperand.GetIntValue();
                }

            }
        }

    }
}