using System.Runtime.CompilerServices;
using compiler_construction.Syntax;
using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Interpretation;

public class FactorInterpreter : Interpretable
{
    private FactorNode _factor;

    public FactorInterpreter(FactorNode factor)
    {
        Debug.Log($"Factor is not null: {factor != null}");
        _factor = factor;
        children = factor.GetChildren();
    }
    public override void Interpret()
    {
        Debug.Log("Started to interpret factor");
        List<TreeNode> operands = _factor.GetChildren();
        List<TermInterpreter> termInterpreters = new List<TermInterpreter>(); 
        List<Token> operators = _factor.GetOperators();

        bool allStrings = true; // then we can concatenate
        bool allArrays = true; // then we can construct a new array
        bool allTuples = true; // then we can construct a new tuple
        bool allNumerical = true; // then we just calculate
        foreach (TermNode node in operands)
        {
            TermInterpreter termInterpreter = new TermInterpreter(node);
            termInterpreter.Interpret();
            termInterpreters.Add(termInterpreter);

            if (termInterpreter.GetWhatExpression() is WhatExpression.IntegerExpr || termInterpreter.GetWhatExpression() is WhatExpression.RealExpr
                ||  termInterpreter.GetWhatExpression() is WhatExpression.BoolExpr)
            {
                allStrings = false;
                allArrays = false;
                allTuples = false;
            }

            if (termInterpreter.GetWhatExpression() is WhatExpression.StringExpr)
            {
                allArrays = false;
                allTuples = false;
                allNumerical = false;
            }

            if (termInterpreter.GetWhatExpression() is WhatExpression.ArrayExpr)
            {
                allStrings = false;
                allTuples = false;
                allNumerical = false;
            }

            if (termInterpreter.GetWhatExpression() is WhatExpression.TupleExpr)
            {
                allArrays = false;
                allStrings = false;
                allNumerical = false;
            }

            if (!allStrings && !allArrays && !allTuples && !allNumerical)
            {
                throw new InterpretationException("Interpretation: conflicting values, cannot apply factor operators");
            }
        }
        Debug.Log("there are " + _factor.GetChildren().Count + " children for the factor");
        Debug.Log("there are " + termInterpreters.Count() + " term interpreters");
        if (termInterpreters.Count == 1)
        {
            Debug.Log("TermInterpreter:" +  " " + termInterpreters.First().GetIntValue() + " " + termInterpreters.First().GetRealValue());
            Debug.Log("Factor consists of only one term");
            InheritValues(termInterpreters.First(), "Interpretation: cannot interpret factor while inheriting from a single term");
        }
        else
        {
            // case if we are concatenating strings
            if (allStrings)
            {
                WhatExpr = WhatExpression.StringExpr;
                string concatenatedString = "";
                foreach (Token token in operators)
                {
                    if (token is Minus)
                    {
                        throw new InterpretationException("Interpretation: operator '-' cannot be applied to strings");
                    }
                }

                foreach (TermInterpreter termInterpreter in termInterpreters)
                {
                    string newString = termInterpreter.GetStringValue();
                    concatenatedString += newString;
                }

                StringValue = concatenatedString;
            }
            else if (allArrays)
            {
                WhatExpr = WhatExpression.ArrayExpr;
                List<ExpressionNode> concatenatedArray = new List<ExpressionNode>();
                foreach (Token token in operators)
                {
                    if (token is Minus)
                    {
                        throw new InterpretationException("Interpretation: operator '-' cannot be applied to arrays");
                    }
                }

                foreach (TermInterpreter termInterpreter in termInterpreters)
                {
                    List<ExpressionNode> newArray = termInterpreter.GetArrayValue();
                    concatenatedArray.AddRange(newArray);
                }

                ArrayValue = concatenatedArray;
                
                List<TreeNode> newChildren = new List<TreeNode>();
                
                TreeNode node = new ArrayNode();
                
                foreach (ExpressionNode element in concatenatedArray)
                {
                    node.AddChild(element);
                }
                node = new LiteralNode().AddChild(node);
                node = new PrimaryNode().AddChild(node);
                node = new UnaryNode().AddChild(node);
                node = new TermNode().AddChild(node);
                node = new FactorNode().AddChild(node);
                node = new RelationNode().AddChild(node);
                
                newChildren.Add(node);
                
                children = newChildren;
                
            }
            else if (allTuples)
            {
                WhatExpr = WhatExpression.TupleExpr;
                List<TupleElementNode> concatenatedTuple = new List<TupleElementNode>();
                foreach (Token token in operators)
                {
                    if (token is Minus)
                    {
                        throw new InterpretationException("Interpretation: operator '-' cannot be applied to tuples");
                    }
                }

                foreach (TermInterpreter termInterpreter in termInterpreters)
                {
                    List<TupleElementNode> newTuple = termInterpreter.GetTupleValue();
                    concatenatedTuple.AddRange(newTuple);
                }
                List<TreeNode> newChildren = new List<TreeNode>();
                TupleValue = concatenatedTuple;
                
                TreeNode node = new TupleNode();
                
                foreach (TupleElementNode element in concatenatedTuple)
                {
                    node.AddChild(element);
                }
                node = new LiteralNode().AddChild(node);
                node = new PrimaryNode().AddChild(node);
                node = new UnaryNode().AddChild(node);
                node = new TermNode().AddChild(node);
                node = new FactorNode().AddChild(node);
                node = new RelationNode().AddChild(node);
                
                newChildren.Add(node);
                children = newChildren;
            }
            else if (allNumerical)
            {
                bool hasReal = false;
                bool hasBool = false;
                foreach (TermInterpreter termInterpreter in termInterpreters)
                {
                    if (termInterpreter.GetWhatExpression() is WhatExpression.BoolExpr)
                    {
                        hasBool = true;
                    }
                    else if (termInterpreter.GetWhatExpression() is WhatExpression.RealExpr)
                    {
                        hasReal = true;
                    }
                }
                
                if (hasBool)
                {
                    if (operators.Count > 0)
                    {
                        throw new InterpretationException("Cannot add or subtract with booleans");
                    }

                    WhatExpr = WhatExpression.BoolExpr;
                    BoolValue = termInterpreters.First().GetBoolValue();
                    return;
                }

                WhatExpr = hasReal ? WhatExpression.RealExpr : WhatExpression.IntegerExpr;
                
                if (termInterpreters.First().GetWhatExpression() is WhatExpression.RealExpr) RealValue = 
                    termInterpreters.First().GetRealValue();
                else IntValue = termInterpreters.First().GetIntValue();
                
                if (WhatExpr is WhatExpression.RealExpr && WhatExpr != termInterpreters.First().GetWhatExpression())
                {
                    RealValue = IntValue;
                }
                
                for (int i = 0; i < operators.Count; i++)
                {
                    var op = operators[i];
                    var rightOperand = termInterpreters[i + 1];
            
                    if (WhatExpr == WhatExpression.RealExpr)
                    {
                        double rightOperandValue = rightOperand.GetWhatExpression() == WhatExpression.RealExpr
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

    }
}