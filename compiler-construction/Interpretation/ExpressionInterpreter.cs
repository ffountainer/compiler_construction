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
        children = expression.GetChildren();
    }
    
    private List<TreeNode> operands = [];
    private List<Token> operators = [];

    public override void Interpret()
    {
        Debug.Log(">>>>>>>>>>>> IM INTERPRETING EXPRESSION");
        operands = _expression.GetChildren();
        List<RelationInterpreter> relationInterpreters = new List<RelationInterpreter>(); 
        operators = _expression.GetOperators();
        
        foreach (RelationNode node in operands)
        {
            RelationInterpreter relationInterpreter = new RelationInterpreter(node);
            relationInterpreters.Add(relationInterpreter);
            relationInterpreter.Interpret();

            if (relationInterpreter.GetWhatExpression() != WhatExpression.BoolExpr && relationInterpreters.Count > 1)
            {
                throw new InterpretationException("Cannot apply logical operator to a none-boolean relations");
            }
        }
        if (relationInterpreters.Count == 1)
        {
            Debug.Log("Expression consists of only one relation");
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
        switch (WhatExpr)
        {
            case(WhatExpression.IntegerExpr):
                Console.Write(IntValue + " ");
                break;
            case(WhatExpression.RealExpr):
                Console.Write(RealValue + " ");
                break;
            case(WhatExpression.BoolExpr):
                Console.Write(BoolValue + " ");
                break;
            case(WhatExpression.StringExpr):
                Console.Write(StringValue + " ");
                break;
            case(WhatExpression.ArrayExpr):
                Console.Write("[ ");
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
                            Console.Write("[ ");
                            foreach (var innerElementArray in elementInterpreter.GetArrayValue())
                            {
                                ExpressionInterpreter innerElementArrayInterpreter = new ExpressionInterpreter(innerElementArray);
                                innerElementArrayInterpreter.Interpret();
                                innerElementArrayInterpreter.PrintExpression();
                            }
                            Console.Write("] ");
                            break;
                        case(WhatExpression.TupleExpr):
                            Console.Write("{ ");
                            foreach (var innerTupleArray in elementInterpreter.TupleValue)
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
                            Console.Write("} ");
                            break;
                        case(WhatExpression.NoneExpr):
                            throw new InterpretationException("Cannot print a none value");
                        case(WhatExpression.FuncLiteralExpr):
                            throw new InterpretationException("Cannot print a function literal");
                        default:
                            throw new InterpretationException("Cannot print an array");
                    }
                }
                Console.Write("] ");
                break;
            case(WhatExpression.TupleExpr):
                Console.Write("{ ");
                foreach (var element in TupleValue)
                {
                    IdentifierNode ident = element.key;
                    ExpressionNode value = element.value;
                    
                    if (ident != null)
                    {
                        Console.Write(ident.GetValue() + ":");
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
                            Console.Write("[ ");
                            foreach (var innerElementArray in elementInterpreter.ArrayValue)
                            {
                                ExpressionInterpreter innerElementArrayInterpreter = new ExpressionInterpreter(innerElementArray);
                                innerElementArrayInterpreter.Interpret();
                                innerElementArrayInterpreter.PrintExpression();
                            }
                            Console.Write("] ");
                            break;
                        case(WhatExpression.TupleExpr):
                            Console.Write("{ ");
                            foreach (var innerTupleArray in elementInterpreter.TupleValue)
                            {
                                ExpressionNode innerElementTuple = innerTupleArray.value;
                                IdentifierNode key = innerTupleArray.key;
                                ExpressionInterpreter innerElementTupleInterpreter = new ExpressionInterpreter(innerElementTuple);
                                innerElementTupleInterpreter.Interpret();
                                if (key != null)
                                {
                                    Console.Write(key.GetValue() + ":");
                                }
                                innerElementTupleInterpreter.PrintExpression();
                            }
                            Console.Write("} ");
                            break;
                        case(WhatExpression.NoneExpr):
                            throw new InterpretationException("Cannot print a none value");
                        case(WhatExpression.FuncLiteralExpr):
                            throw new InterpretationException("Cannot print a function literal");
                        default:
                            throw new InterpretationException("Cannot print an array");
                    }
                }
                Console.Write("} ");
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