using compiler_construction.Syntax;
using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class ArrayInterpreter : Interpretable
{
    private ArrayNode _array;

    public ArrayInterpreter(ArrayNode array)
    {
        _array = array;
        children = array.GetChildren();
    }
    public override void Interpret()
    {
        WhatExpr = WhatExpression.ArrayExpr;
        foreach (var expr in _array.GetChildren())
        {
            ExpressionInterpreter expressionInterpreter = new ExpressionInterpreter((ExpressionNode)expr);
            expressionInterpreter.Interpret();
            ArrayValue.Add(ConstructExpressionFromExprInterpreter(expressionInterpreter));
        }
    }
}