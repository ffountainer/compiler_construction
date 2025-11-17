using compiler_construction.Syntax;
using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class ArrayInterpreter : Interpretable
{
    private ArrayNode _array;

    public ArrayInterpreter(ArrayNode array)
    {
        _array = array;
    }
    public override void Interpret()
    {
        WhatExpr = WhatExpression.ArrayExpr;
        foreach (var expr in _array.GetChildren())
        {
            ArrayValue.Add((ExpressionNode)expr);
        }
    }
}