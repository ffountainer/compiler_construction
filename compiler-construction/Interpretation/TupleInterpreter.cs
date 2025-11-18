using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class TupleInterpreter : Interpretable
{
    private TupleNode _tuple;

    public TupleInterpreter(TupleNode tuple)
    {
        _tuple = tuple;
        children = tuple.GetChildren();
    }
    public override void Interpret()
    {
        Debug.Log("Started interpreting tuple");
        WhatExpr = WhatExpression.TupleExpr;
        foreach (TupleElementNode tupleElement in _tuple.GetChildren())
        {
            var ident = tupleElement.key;
            var expr = tupleElement.value;
            TupleValue.Add(new TupleElementNode(ident, expr, tupleElement.GetChildren()));
        }
    }
}