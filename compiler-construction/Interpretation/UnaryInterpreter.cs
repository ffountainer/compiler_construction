using compiler_construction.Syntax;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Interpretation;

public class UnaryInterpreter : Interpretable
{
    private UnaryNode _unary;

    public UnaryInterpreter(UnaryNode node)
    {
        _unary = node;
        children = node.GetChildren();
    }
    
    public override void Interpret()
    {
        Debug.Log("Started to interpret unary");
        // case Reference
        if (_unary.GetChildren().Count == 1 && _unary.GetChildren().First() is ReferenceNode reference)
        {
            ReferenceInterpreter referenceInterpreter = new ReferenceInterpreter(reference);
            referenceInterpreter.Interpret();
            InheritValues(referenceInterpreter, $"Interpretation: error to interpret unary from {reference.GetIdentifier().GetValue()}");
        }
        // case Reference is TypeIndicator
        else if (_unary.GetChildren().First() is ReferenceNode referenceNode)
        {
            WhatExpr = WhatExpression.BoolExpr;
            ReferenceInterpreter referenceNodeInterpreter = new ReferenceInterpreter(referenceNode);
            referenceNodeInterpreter.Interpret();
            TypeIndicatorNode type = (TypeIndicatorNode)_unary.GetChildren().Skip(1).First();
            string typeName = type.GetName();
            WhatExpression referenceType = referenceNodeInterpreter.GetWhatExpression();
            if ((referenceType is WhatExpression.IntegerExpr && typeName.Equals("int"))
                || (referenceType is WhatExpression.StringExpr && typeName.Equals("string"))
                || (referenceType is WhatExpression.RealExpr &&  typeName.Equals("real"))
                || (referenceType is WhatExpression.BoolExpr && typeName.Equals("bool"))
                || (referenceType is WhatExpression.NoneExpr &&  typeName.Equals("no type"))
                || (referenceType is WhatExpression.FuncLiteralExpr &&  typeName.Equals("func"))
                || (referenceType is WhatExpression.ArrayExpr &&  typeName.Equals("vector type"))
                || (referenceType is WhatExpression.TupleExpr &&  typeName.Equals("tuple")))
            {
                BoolValue = true;
            }
            else
            {
                BoolValue = false;
            }
        }
        // case [ + | - | not ] Primary
        else
        {
            PrimaryNode primary = (PrimaryNode)_unary.GetChildren().First();
            PrimaryInterpreter primaryInterpreter = new PrimaryInterpreter(primary);
            primaryInterpreter.Interpret();
            InheritValues(primaryInterpreter, "Interpretation: error interpreting unary while inheriting from primary");
            
            Token? primaryOperator = _unary.GetPrimaryOperator();
            
            if (_unary.GetPrimaryOperator() != null)
            {
                switch (primaryOperator)
                {
                    case(Plus):
                        switch (WhatExpr)
                        {
                            case(WhatExpression.IntegerExpr):
                                break;
                            case(WhatExpression.RealExpr):
                                break;
                            default:
                                throw new InterpretationException($"Interpretation: you cannot apply Plus to {WhatExpr}");
                        }
                        break;
                    case(Minus):
                        switch (WhatExpr)
                        {
                            case(WhatExpression.IntegerExpr):
                                IntValue = -IntValue;
                                break;
                            case(WhatExpression.RealExpr):
                                RealValue = -RealValue;
                                break;
                            default:
                                throw new InterpretationException($"Interpretation: you cannot apply Minus to {WhatExpr}");
                        }

                        break;
                    case(Not):
                        switch (WhatExpr)
                        {
                            case(WhatExpression.IntegerExpr):
                                WhatExpr = WhatExpression.BoolExpr;
                                if (IntValue == 0)
                                {
                                    BoolValue = true;
                                }
                                else if (IntValue == 1)
                                {
                                    BoolValue = false;
                                }
                                else
                                {
                                    throw new InterpretationException($"Interpretation: you cannot apply Not to {WhatExpression.IntegerExpr}");
                                }
                                break;
                            case(WhatExpression.BoolExpr):
                                if (!BoolValue)
                                {
                                    BoolValue = true;
                                }
                                else
                                {
                                    BoolValue = false;
                                }
                                break;
                            default:
                                throw new InterpretationException($"Interpretation: you cannot apply Not to {WhatExpr}");
                        }
                        break;
                    default:
                        throw new InterpretationException($"Interpretation: error trying to interpret Unary");
                }
            }
        }
    }
}