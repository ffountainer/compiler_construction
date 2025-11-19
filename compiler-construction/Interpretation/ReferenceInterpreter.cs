using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class ReferenceInterpreter : Interpretable
{
    private ReferenceNode _reference;

    public bool isTypeCheck = false;

    public ReferenceInterpreter(ReferenceNode reference, bool isCheck)
    {
        _reference = reference;
        children = reference.GetChildren();
        isTypeCheck = isCheck;
    }
    public override void Interpret()
    {
        Debug.Log("Started to interpret reference");
        IdentifierNode referenceIdent =  (IdentifierNode)_reference.GetChildren().First();
        switch (_reference.getWhatReference())
        {
            case(WhatReference.Ident):
                Debug.Log("The reference is by ident");
                ExpressionNode expr = FindExpression(referenceIdent);
                if (expr == null && !isTypeCheck)
                {
                    throw new InterpretationException(
                        $"Interpretation: cannot reference a null value from identifier {referenceIdent.GetValue()}");
                }

                if (expr == null && isTypeCheck)
                {
                    expr = ConstructNullExpr();
                }
                ExpressionInterpreter exprInterpreter = new ExpressionInterpreter(expr);
                exprInterpreter.Interpret();
                InheritValues(exprInterpreter, $"Interpretation: error trying to interpret reference for {referenceIdent.GetValue()}");
                break;
            case(WhatReference.Array):
                Debug.Log("The reference is by array element");
                ExpressionNode referencedArray = FindExpression(referenceIdent);
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
                InheritValues(arrayElementInterpreter, "Interpretation: cannot interpret array element");
                break;
            case(WhatReference.Tuple):
                Debug.Log("The reference is by tuple");
                ExpressionNode referencedTuple = FindExpression(referenceIdent);
                if (referencedTuple == null)
                {
                    throw new InterpretationException(
                        $"Interpretation: cannot reference a null value from identifier {referenceIdent.GetValue()}");
                }
                Debug.Log($"Reference: got tuple with name {referenceIdent.GetValue()}");
                Debug.Log("Starting to interpret tuple expression");
                ExpressionInterpreter tupleInterpreter = new ExpressionInterpreter(referencedTuple);
                tupleInterpreter.Interpret();
                Debug.Log("Finished to interpret tuple expression");
                
                switch (_reference.GetWhatTupleReference())
                {
                    case(WhatTupleReference.TupleByIndex):
                        Debug.Log("The tuple reference is by index");
                        int index = _reference.GetTupleIndexInt();
                        var tupleElementByInd = GetTupleElementByIndex(tupleInterpreter.GetTupleValue(), index - 1);
                        ExpressionInterpreter tupleElementByIndInterpreter = new ExpressionInterpreter(tupleElementByInd);
                        tupleElementByIndInterpreter.Interpret();
                        InheritValues(tupleElementByIndInterpreter, "Interpretation: cannot interpret tuple element (accessed by index)");
                        break;
                    case(WhatTupleReference.TupleByIdent):
                        Debug.Log("The tuple reference is by ident");
                        IdentifierNode accessIdent =  _reference.GetTupleAccessIdentifier();
                        var tupleElementByKey = GetTupleElementByKey(tupleInterpreter.GetTupleValue(), accessIdent);
                        ExpressionInterpreter tupleElementByKeyInterpreter = new ExpressionInterpreter(tupleElementByKey);
                        tupleElementByKeyInterpreter.Interpret();
                        InheritValues(tupleElementByKeyInterpreter, "Interpretation: cannot interpret tuple element (accessed by key)");
                        break;
                    default:
                        throw new InterpretationException($"Interpretation: cannot interpret a reference for tuple with identifier {referenceIdent.GetValue()}");
                }
                break;
            case(WhatReference.Call):
                ExpressionNode referencedFunction = FindExpression(referenceIdent);
                if (referencedFunction == null)
                {
                    throw new InterpretationException(
                        $"Interpretation: cannot reference a null value from identifier {referenceIdent.GetValue()}");
                }
                ExpressionInterpreter functionLiteralInterpreter = new ExpressionInterpreter(referencedFunction);
                functionLiteralInterpreter.Interpret();

                List<TreeNode> arguments = _reference.GetChildren().Skip(1).First().GetChildren().ToList();
                FunctionInterpreter functionInterpreter = new FunctionInterpreter(functionLiteralInterpreter.GetBody(), 
                    functionLiteralInterpreter.GetArguments(), functionLiteralInterpreter.GetWhatFunc(), 
                    functionLiteralInterpreter.GetShortFuncExpr(), arguments);
                functionInterpreter.Interpret();
                break;
            default:
                throw new InterpretationException($"Interpretation: error trying to interpret a reference {referenceIdent.GetValue()}");
        }
    }
}