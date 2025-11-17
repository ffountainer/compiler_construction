using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class ExpressionInterpreter : Interpretable
{
    private ExpressionNode _expression;

    public ExpressionInterpreter(ExpressionNode expression)
    {
        _expression = expression;
    }

    public override void Interpret()
    {
        if (_expression.GetChildren().Count > 1)
        {
            // logic for or/and/xor
            List<Object> values = new List<Object>();
            foreach (RelationNode relation in _expression.GetChildren())
            {
                RelationInterpreter relationInterpreter = new RelationInterpreter(relation);
                values.Add(relationInterpreter.Interpret());
            }
        }
        else
        {
            
        }
        
    }

    public void PrintExpression()
    {
        switch (_expression.WhatExpression)
        {
            case(WhatExpression.IntegerExpr):
                Console.WriteLine(_expression.GetIntValue());
                break;
            case(WhatExpression.RealExpr):
                Console.WriteLine(_expression.GetRealValue());
                break;
            case(WhatExpression.BoolExpr):
                Console.WriteLine(_expression.GetBoolValue());
                break;
            case(WhatExpression.StringExpr):
                // TODO: add string var
                break;
            case(WhatExpression.ArrayExpr):
                // TODO: add array var
                break;
            case(WhatExpression.TupleExpr):
                // TODO: add tuple var
                break;
        }
    }
}