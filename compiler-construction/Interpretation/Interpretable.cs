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
    protected List<ExpressionNode> ArrayValue = new List<ExpressionNode>();
    protected List<TupleElementNode> TupleValue = new List<TupleElementNode>();
    protected List<String> Arguments = new List<string>();
    protected WhatFunction WhatFunc;
    protected List<StatementNode> Body = new List<StatementNode>();
    protected ExpressionNode shortFuncExpr;
    protected ExpressionNode bracedExpr;
    protected List<TreeNode> children = new List<TreeNode>();
    
    public List<TreeNode> GetChildren()
    {
        return children;
    }

    public void InheritValues(Interpretable interpreter, string exception)
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
    
    public  ExpressionNode ConstructNullExprArray()
    {
        TreeNode expr = new NoneLiteral();
        expr = new LiteralNode().AddChild(expr);
        expr = new PrimaryNode().AddChild(expr);
        expr = new UnaryNode().AddChild(expr);
        expr = new TermNode().AddChild(expr);
        expr = new FactorNode().AddChild(expr);
        expr = new RelationNode().AddChild(expr);
        expr = new ExpressionNode().AddChild(expr);
        TreeNode node = new ArrayNode();
        node.AddChild(expr);
        node = new LiteralNode().AddChild(node);
        node = new PrimaryNode().AddChild(node);
        node = new UnaryNode().AddChild(node);
        node = new TermNode().AddChild(node);
        node = new FactorNode().AddChild(node);
        node = new RelationNode().AddChild(node);

        ExpressionNode nullExpr = (ExpressionNode)new ExpressionNode().AddChild(node);
        return nullExpr;
    }
    
    public  ExpressionNode ConstructNullExpr()
    {
        TreeNode node = new NoneLiteral();
        node = new LiteralNode().AddChild(node);
        node = new PrimaryNode().AddChild(node);
        node = new UnaryNode().AddChild(node);
        node = new TermNode().AddChild(node);
        node = new FactorNode().AddChild(node);
        node = new RelationNode().AddChild(node);
        
        ExpressionNode nullExpr = (ExpressionNode)new ExpressionNode().AddChild(node);
        return nullExpr;
    }
        
    public ExpressionNode GetTupleElementByKey(List<TupleElementNode> tupleValue, IdentifierNode findKey)
    {
        Debug.Log($"The size of search tuple is {tupleValue.Count}");
        foreach (var item in tupleValue)
        {
            if (item.key != null)
            {
                Debug.Log($"trying to find key {findKey.GetValue()}, checking against {item.key.GetValue()}");
                if (item.key.GetValue().Equals(findKey.GetValue()))
                {
                    return item.value; 
                }
            }
        }
        throw new InterpretationException($"Interpretation: No element found with the given key {findKey.GetValue()}");
    }
    
    public double GetNumericalValue()
    {
        if (WhatExpr != WhatExpression.RealExpr && WhatExpr != WhatExpression.IntegerExpr)
        {
            throw new InterpretationException("The factor does not have a numeric value");
        }
        
        return WhatExpr ==  WhatExpression.RealExpr ? RealValue : IntValue;
    }
    
    public ExpressionNode GetTupleElementByIndex(List<TupleElementNode> tupleValue, int index)
    {
        return tupleValue[index].value;
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