using compiler_construction.Semantics;
using compiler_construction.Syntax;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Interpretation;

public class RelationInterpreter : Interpretable
{
    private RelationNode _relation;

    public RelationInterpreter(RelationNode relation)
    {
        _relation = relation;
        children = relation.GetChildren();
    }

    public override void Interpret()
    {
        Debug.Log("Starting to interpret relation");
        FactorNode lhs = (FactorNode)_relation.GetChildren().First();
        FactorNode rhs = null;
        if (_relation.GetChildren().Count() > 1)
        {
            rhs = (FactorNode) _relation.GetChildren().Skip(1).First();
        }
        
        FactorInterpreter lhsInterpreter = new FactorInterpreter(lhs);
        
        lhsInterpreter.Interpret();
        if (rhs is null)
        {
            Debug.Log("RHS is null");
            InheritValues(lhsInterpreter, "Interpreter: error interpreting relation while inheriting from factor");
        }
        else
        {
            Token op = _relation.GetTheOperator();
            Debug.Log("RHS is not null!");
            FactorInterpreter rhsInterpreter = new FactorInterpreter(rhs);
            rhsInterpreter.Interpret();
            WhatExpr = WhatExpression.BoolExpr;
            
            if (op is Equal || op is NotEqual)
            {

                if (lhsInterpreter.GetWhatExpression() == WhatExpression.BoolExpr ||
                    rhsInterpreter.GetWhatExpression() == WhatExpression.BoolExpr)
                {
                    var equal = lhsInterpreter.GetWhatExpression() == rhsInterpreter.GetWhatExpression() &&
                                lhsInterpreter.GetBoolValue() == rhsInterpreter.GetBoolValue();
                    BoolValue = op is Equal ? equal : !equal;
                }
                else
                {
                    var equal = lhsInterpreter.GetNumericalValue() == rhsInterpreter.GetNumericalValue();
                    BoolValue = op is Equal ? equal : !equal;
                }
                return;
            }

            if (lhsInterpreter.GetWhatExpression() == WhatExpression.BoolExpr ||
                rhsInterpreter.GetWhatExpression() == WhatExpression.BoolExpr)
            {
                throw new InterpretationException("Cannot compare boolean values as numbers");
            }
            
            var lhsValue = lhsInterpreter.GetNumericalValue();
            var rhsValue = rhsInterpreter.GetNumericalValue();
           
            if (op is Greater) BoolValue =  lhsValue > rhsValue;
            else if (op is Less) BoolValue = lhsValue < rhsValue;
            else if (op is LessEqual) BoolValue = lhsValue <= rhsValue;
            else if (op is GreaterEqual) BoolValue = lhsValue >= rhsValue;
        }
    }
}