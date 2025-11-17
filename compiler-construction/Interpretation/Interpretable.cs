using compiler_construction.Syntax;
using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public abstract class Interpretable
{
    protected WhatExpression WhatExpr;
    protected bool BoolValue;
    protected double RealValue;
    protected int IntValue;
    protected string StringValue;
    protected List<ExpressionNode> ArrayValue;
    protected List<TupleElementNode> TupleValue;
    protected List<String> Arguments;
    protected WhatFunction WhatFunc;
    protected List<StatementNode> Body;
    protected ExpressionNode shortFuncExpr;
    protected ExpressionNode bracedExpr;

    public void InheritValuesFromExpr(Interpretable interpreter, string exception)
    {
        switch (interpreter.GetWhatExpression())
        {
            case (WhatExpression.IntegerExpr):
                WhatExpr = WhatExpression.IntegerExpr;
                IntValue = interpreter.GetIntValue();
                break;
            case (WhatExpression.RealExpr):
                WhatExpr = WhatExpression.RealExpr;
                RealValue = interpreter.GetRealValue();
                break;
            case (WhatExpression.StringExpr):
                WhatExpr = WhatExpression.StringExpr;
                StringValue = interpreter.GetStringValue();
                break;
            case (WhatExpression.BoolExpr):
                WhatExpr = WhatExpression.BoolExpr;
                BoolValue = interpreter.GetBoolValue();
                break;
            case (WhatExpression.NoneExpr):
                WhatExpr = WhatExpression.NoneExpr;
                break;
            case (WhatExpression.ArrayExpr):
                WhatExpr = WhatExpression.ArrayExpr;
                ArrayValue = interpreter.GetArrayValue();
                break;
            case (WhatExpression.TupleExpr):
                WhatExpr = WhatExpression.TupleExpr;
                TupleValue = interpreter.GetTupleValue();
                break;
            case (WhatExpression.FuncLiteralExpr):
                WhatExpr = WhatExpression.FuncLiteralExpr;
                WhatFunc = interpreter.GetWhatFunc();
                Arguments = interpreter.GetArguments();
                if (WhatFunc is WhatFunction.Full)
                {
                    Body = interpreter.GetBody();
                }
                else
                {
                    shortFuncExpr = interpreter.GetShortFuncExpr();
                }
                break;
            default:
                throw new InterpretationException(exception);
        }
    }
        
    public ExpressionNode GetTupleElementByKey(IdentifierNode findKey)
    {
        foreach (var item in TupleValue)
        {
            if (item.key != null && item.key.GetValue().Equals(findKey.GetValue()))
            {
               return item.value; 
            }
        }
        throw new InterpretationException($"Interpretation: No element found with the given key {findKey.GetValue()}");
    }
    
    public ExpressionNode GetTupleElementByIndex(int index)
    {
        return TupleValue[index].value;
    }
    
    public ExpressionNode GetShortFuncExpr()
    {
        return shortFuncExpr;
    }

    public List<StatementNode> GetBody()
    {
        return Body;
    }
    
    public List<String> GetArguments()
    {
        return Arguments;
    }

    public WhatFunction GetWhatFunc()
    {
        return WhatFunc;
    }

    public WhatExpression GetWhatExpression()
    {
        return WhatExpr;
    }

    public bool GetBoolValue()
    {
        return BoolValue;
    }

    public double GetRealValue()
    {
        return RealValue;
    }

    public int GetIntValue()
    {
        return IntValue;
    }

    public string GetStringValue()
    {
        return StringValue;
    }

    public List<ExpressionNode> GetArrayValue()
    {
        return ArrayValue;
    }

    public List<TupleElementNode> GetTupleValue()
    {
        return TupleValue;
    }
    
    public abstract void Interpret();
}