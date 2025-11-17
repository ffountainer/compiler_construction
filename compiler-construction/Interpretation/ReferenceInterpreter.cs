using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class ReferenceInterpreter : Interpretable
{
    private ReferenceNode _reference;

    public ReferenceInterpreter(ReferenceNode reference)
    {
        _reference = reference;
    }
    public override void Interpret()
    {
        IdentifierNode referenceIdent =  (IdentifierNode)_reference.GetChildren().First();
        switch (_reference.getWhatReference())
        {
            case(WhatReference.Ident):
                ExpressionNode expr = Interpreter.GetIdentifiers()[referenceIdent];
                if (expr == null)
                {
                    throw new InterpretationException(
                        $"Interpretation: cannot reference a null value from identifier {referenceIdent.GetValue()}");
                }
                ExpressionInterpreter exprInterpreter = new ExpressionInterpreter(expr);
                exprInterpreter.Interpret();
                InheritValuesFromExpr(exprInterpreter, $"Interpretation: error trying to interpret reference for {referenceIdent.GetValue()}");
                break;
            case(WhatReference.Array):
                ExpressionNode referencedArray = Interpreter.GetIdentifiers()[referenceIdent];
                if (referencedArray == null)
                {
                    throw new InterpretationException(
                        $"Interpretation: cannot reference a null value from identifier {referenceIdent.GetValue()}");
                }
                ExpressionInterpreter array = new ExpressionInterpreter(referencedArray);
                array.Interpret(); // turns ArrayValue into a list of ExpressionNodes
                ExpressionInterpreter indexInterpreter = new ExpressionInterpreter(_reference.GetArrayIndex());
                indexInterpreter.Interpret();
                if (!(indexInterpreter.GetWhatExpression() is WhatExpression.IntegerExpr))
                {
                    throw new InterpretationException(
                        $"Interpretation: index of an array element cannot be {indexInterpreter.GetWhatExpression()}");
                }
                var arrayElement = array.GetArrayValue()[indexInterpreter.GetIntValue() - 1];
                ExpressionInterpreter arrayElementInterpreter = new ExpressionInterpreter(arrayElement);
                arrayElementInterpreter.Interpret();
                InheritValuesFromExpr(arrayElementInterpreter, "Interpretation: cannot interpret array element");
                break;
            case(WhatReference.Tuple):
                // TODO: RECHECK AND FINISH
                ExpressionNode referencedTuple = Interpreter.GetIdentifiers()[referenceIdent];
                if (referencedTuple == null)
                {
                    throw new InterpretationException(
                        $"Interpretation: cannot reference a null value from identifier {referenceIdent.GetValue()}");
                }
                ExpressionInterpreter tupleInterpreter = new ExpressionInterpreter(referencedTuple);
                tupleInterpreter.Interpret();
                switch (_reference.GetWhatTupleReference())
                {
                    case(WhatTupleReference.TupleByIndex):
                        int index = _reference.GetTupleIndexInt();
                        var tupleElementByInd = GetTupleElementByIndex(index - 1);
                        ExpressionInterpreter tupleElementByIndInterpreter = new ExpressionInterpreter(tupleElementByInd);
                        tupleElementByIndInterpreter.Interpret();
                        InheritValuesFromExpr(tupleElementByIndInterpreter, "Interpretation: cannot interpret tuple element (accessed by index)");
                        break;
                    case(WhatTupleReference.TupleByIdent):
                        IdentifierNode accessIdent =  _reference.GetTupleAccessIdentifier();
                        var tupleElementByKey = GetTupleElementByKey(accessIdent);
                        ExpressionInterpreter tupleElementByKeyInterpreter = new ExpressionInterpreter(tupleElementByKey);
                        tupleElementByKeyInterpreter.Interpret();
                        InheritValuesFromExpr(tupleElementByKeyInterpreter, "Interpretation: cannot interpret tuple element (accessed by key)");
                        break;
                    default:
                        throw new InterpretationException($"Interpretation: cannot interpret a reference for tuple with identifier {referenceIdent.GetValue()}");
                }
                break;
                default:
                    throw new InterpretationException($"Interpretation: error trying to interpret a reference {referenceIdent.GetValue()}");
        }
    }
}