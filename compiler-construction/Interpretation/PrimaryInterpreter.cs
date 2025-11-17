using compiler_construction.Syntax;
using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class PrimaryInterpreter : Interpretable
{
    private PrimaryNode _primary;

    public PrimaryInterpreter(PrimaryNode primary)
    {
        _primary = primary;
    }
    public override void Interpret()
    {
        var child = _primary.GetChildren().First();
        switch (child)
        {
            case(LiteralNode node):
                var literalInterpreter =  new LiteralInterpreter(node);
                literalInterpreter.Interpret();
                switch (literalInterpreter.GetWhatExpression())
                {
                    case(WhatExpression.IntegerExpr):
                        WhatExpr = WhatExpression.IntegerExpr;
                        IntValue = literalInterpreter.GetIntValue();
                        break;
                    case(WhatExpression.RealExpr):
                        WhatExpr = WhatExpression.RealExpr;
                        RealValue = literalInterpreter.GetRealValue();
                        break;
                    case(WhatExpression.StringExpr):
                        WhatExpr = WhatExpression.StringExpr;
                        StringValue = literalInterpreter.GetStringValue();
                        break;
                    case(WhatExpression.BoolExpr):
                        WhatExpr = WhatExpression.BoolExpr;
                        BoolValue = literalInterpreter.GetBoolValue();
                        break;
                    case(WhatExpression.NoneExpr):
                        WhatExpr = WhatExpression.NoneExpr;
                        break;
                    case(WhatExpression.ArrayExpr):
                        WhatExpr = WhatExpression.ArrayExpr;
                        ArrayValue = literalInterpreter.GetArrayValue();
                        break;
                    case(WhatExpression.TupleExpr):
                        WhatExpr = WhatExpression.TupleExpr;
                        TupleValue = literalInterpreter.GetTupleValue();
                        break;
                    default:
                        throw new InterpretationException("Interpretation: the literal is not supported");
                }
                break;
            case(FunctionLiteralNode node):
                WhatExpr = WhatExpression.FuncLiteralExpr;
                WhatFunc = node.GetWhatFunc();
                Arguments = node.GetArguments();
                if (WhatFunc is WhatFunction.Full)
                {
                    Body = node.GetBody();
                }
                else
                {
                    shortFuncExpr = node.GetShortFuncExpr();
                }
                break;
            case(ExpressionNode node):
                ExpressionInterpreter expressionInterpreter = new ExpressionInterpreter(node);
                expressionInterpreter.Interpret();
                switch (expressionInterpreter.GetWhatExpression())
                {
                    case(WhatExpression.IntegerExpr):
                        WhatExpr = WhatExpression.IntegerExpr;
                        IntValue = expressionInterpreter.GetIntValue();
                        break;
                    case(WhatExpression.RealExpr):
                        WhatExpr = WhatExpression.RealExpr;
                        RealValue = expressionInterpreter.GetRealValue();
                        break;
                    case(WhatExpression.StringExpr):
                        WhatExpr = WhatExpression.StringExpr;
                        StringValue = expressionInterpreter.GetStringValue();
                        break;
                    case(WhatExpression.BoolExpr):
                        WhatExpr = WhatExpression.BoolExpr;
                        BoolValue = expressionInterpreter.GetBoolValue();
                        break;
                    case(WhatExpression.NoneExpr):
                        WhatExpr = WhatExpression.NoneExpr;
                        break;
                    case(WhatExpression.ArrayExpr):
                        WhatExpr = WhatExpression.ArrayExpr;
                        ArrayValue = expressionInterpreter.GetArrayValue();
                        break;
                    case(WhatExpression.TupleExpr):
                        WhatExpr = WhatExpression.TupleExpr;
                        TupleValue = expressionInterpreter.GetTupleValue();
                        break;
                    case(WhatExpression.FuncLiteralExpr):
                        WhatExpr = WhatExpression.FuncLiteralExpr;
                        WhatFunc = expressionInterpreter.GetWhatFunc();
                        Arguments = expressionInterpreter.GetArguments();
                        if (WhatFunc is WhatFunction.Full)
                        {
                            Body = expressionInterpreter.GetBody();
                        }
                        else
                        {
                            shortFuncExpr = expressionInterpreter.GetShortFuncExpr();
                        }
                        break;
                    default:
                        throw new InterpretationException("Interpretation: error trying to interpret braced expression");
                }
                break;
            default:
                throw new InterpretationException("Interpretation: the primary cannot be interpreted");
        }
    }
}