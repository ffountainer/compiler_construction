using compiler_construction.Syntax;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Interpretation;

public class ExpressionInterpreter : Interpretable
{
    private ExpressionNode _expression;

    public ExpressionInterpreter(ExpressionNode expression)
    {
        _expression = expression;
    }
    
    private List<TreeNode> operands = [];
    private List<Token> operators = [];

    public override void Interpret()
    { 
        operands = _expression.GetChildren();
        List<RelationInterpreter> relationInterpreters = new List<RelationInterpreter>(); 
        operators = _expression.GetOperators();

        foreach (RelationNode node in operands)
        {
            RelationInterpreter relationInterpreter = new RelationInterpreter(node);
            relationInterpreters.Add(relationInterpreter);
            relationInterpreter.Interpret();

            if (relationInterpreter.GetWhatExpression() != WhatExpression.BoolExpr)
            {
                throw new InterpretationException("Cannot apply logical operator to a none-boolean relations");
            }
        }

        if (relationInterpreters.Count == 1)
        {
            InheritValues(relationInterpreters[0], "Interpretation: error interpreting expression while inheriting from a single relation");
        }
        else
        {
            // Operator priority: Xor > And > Or
            // https://stackoverflow.com/a/12494812

            List<bool> operandValues = [];
            foreach (var relationInterpreter in relationInterpreters)
            {
                operandValues.Add(relationInterpreter.GetBoolValue());
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
                throw new InterpretationException($"{operators.Count} operators left after reduction...");
            }
        
            BoolValue = operandValues[0];
            WhatExpr = WhatExpression.BoolExpr;
        }
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

    public void PrintExpression()
    {
        switch (_expression.WhatExpression)
        {
            case(WhatExpression.IntegerExpr):
                Console.WriteLine(IntValue);
                break;
            case(WhatExpression.RealExpr):
                Console.WriteLine(RealValue);
                break;
            case(WhatExpression.BoolExpr):
                Console.WriteLine(BoolValue);
                break;
            case(WhatExpression.StringExpr):
                Console.WriteLine(StringValue);
                break;
            case(WhatExpression.ArrayExpr):
                foreach (var element in ArrayValue)
                {
                    ExpressionInterpreter elementInterpreter = new ExpressionInterpreter(element);
                    elementInterpreter.Interpret();
                    switch (elementInterpreter.GetWhatExpression())
                    {
                        case(WhatExpression.IntegerExpr):
                            Console.Write(elementInterpreter.GetIntValue() + " ");
                            break;
                        case(WhatExpression.RealExpr):
                            Console.Write(elementInterpreter.GetRealValue() + " ");
                            break;
                        case(WhatExpression.BoolExpr):
                            Console.Write(elementInterpreter.GetBoolValue() + " ");
                            break;
                        case(WhatExpression.StringExpr):
                            Console.Write(elementInterpreter.GetStringValue() + " ");
                            break;
                        case(WhatExpression.ArrayExpr):
                            foreach (var innerElementArray in ArrayValue)
                            {
                                ExpressionInterpreter innerElementArrayInterpreter = new ExpressionInterpreter(innerElementArray);
                                innerElementArrayInterpreter.Interpret();
                                innerElementArrayInterpreter.PrintExpression();
                            }
                            break;
                        case(WhatExpression.TupleExpr):
                            foreach (var innerTupleArray in TupleValue)
                            {
                                ExpressionNode innerElementTuple = innerTupleArray.value;
                                IdentifierNode key = innerTupleArray.key;
                                ExpressionInterpreter innerElementTupleInterpreter = new ExpressionInterpreter(innerElementTuple);
                                innerElementTupleInterpreter.Interpret();
                                if (key != null)
                                {
                                    Console.Write(key.GetValue() + ": ");
                                }
                                innerElementTupleInterpreter.PrintExpression();
                            }
                            break;
                        case(WhatExpression.NoneExpr):
                            throw new InterpretationException("Cannot print a none value");
                        case(WhatExpression.FuncLiteralExpr):
                            throw new InterpretationException("Cannot print a function literal");
                        default:
                            throw new InterpretationException("Cannot print an array");
                    }
                }
                Console.WriteLine();
                break;
            case(WhatExpression.TupleExpr):

                foreach (var element in TupleValue)
                {
                    IdentifierNode ident = element.key;
                    ExpressionNode value = element.value;
                    
                    if (ident != null)
                    {
                        Console.Write(ident.GetValue() + ": ");
                    }
                    
                    ExpressionInterpreter elementInterpreter = new ExpressionInterpreter(value);
                    elementInterpreter.Interpret();
                    switch (elementInterpreter.GetWhatExpression())
                    {
                        case(WhatExpression.IntegerExpr):
                            Console.Write(elementInterpreter.GetIntValue() + " ");
                            break;
                        case(WhatExpression.RealExpr):
                            Console.Write(elementInterpreter.GetRealValue() + " ");
                            break;
                        case(WhatExpression.BoolExpr):
                            Console.Write(elementInterpreter.GetBoolValue() + " ");
                            break;
                        case(WhatExpression.StringExpr):
                            Console.Write(elementInterpreter.GetStringValue() + " ");
                            break;
                        case(WhatExpression.ArrayExpr):
                            foreach (var innerElementArray in ArrayValue)
                            {
                                ExpressionInterpreter innerElementArrayInterpreter = new ExpressionInterpreter(innerElementArray);
                                innerElementArrayInterpreter.Interpret();
                                innerElementArrayInterpreter.PrintExpression();
                            }
                            break;
                        case(WhatExpression.TupleExpr):
                            foreach (var innerTupleArray in TupleValue)
                            {
                                ExpressionNode innerElementTuple = innerTupleArray.value;
                                IdentifierNode key = innerTupleArray.key;
                                ExpressionInterpreter innerElementTupleInterpreter = new ExpressionInterpreter(innerElementTuple);
                                innerElementTupleInterpreter.Interpret();
                                if (key != null)
                                {
                                    Console.Write(key.GetValue() + ": ");
                                }
                                innerElementTupleInterpreter.PrintExpression();
                            }
                            break;
                        case(WhatExpression.NoneExpr):
                            throw new InterpretationException("Cannot print a none value");
                        case(WhatExpression.FuncLiteralExpr):
                            throw new InterpretationException("Cannot print a function literal");
                        default:
                            throw new InterpretationException("Cannot print an array");
                    }
                }
                
                Console.WriteLine();
                break;
            case(WhatExpression.NoneExpr):
                throw new InterpretationException("Cannot print a none value");
            case(WhatExpression.FuncLiteralExpr):
                throw new InterpretationException("Cannot print a function literal");
            default:
                throw new InterpretationException("Cannot print an expression");
        }
    }
}