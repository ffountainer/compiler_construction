using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class LiteralInterpreter : Interpretable
{
    private LiteralNode _literalNode;
    
    public LiteralInterpreter(LiteralNode literalNode)
    {
        _literalNode = literalNode;
        children = literalNode.GetChildren();
    }

    public override void Interpret()
    {
        Debug.Log("Started to interpret Literal Node");
        var firstChild = _literalNode.GetChildren().First();
        switch (firstChild)
        {
            case(IntegerLiteral node):
                IntValue = node.Value;
                WhatExpr = WhatExpression.IntegerExpr;
                Debug.Log($"Interpeted int: Integer value: {IntValue}");
                break;
            case(RealLiteral node):
                RealValue = node.Value;
                WhatExpr = WhatExpression.RealExpr;
                Debug.Log($"Interpeted real: Real: {RealValue}");
                break;
            case(BooleanLiteral node):
                BoolValue = node.Value;
                WhatExpr = WhatExpression.BoolExpr;
                Debug.Log($"Interpeted bool: Bool: {BoolValue}");
                break;
            case(StringLiteral node):
                StringValue = node.Value;
                WhatExpr = WhatExpression.StringExpr;
                Debug.Log($"Interpeted string: String: {StringValue}");
                break;
            case(NoneLiteral):
                WhatExpr = WhatExpression.NoneExpr;
                break;
            case(ArrayNode node):
                WhatExpr = WhatExpression.ArrayExpr;
                var arrayInterpreter = new ArrayInterpreter(node);
                arrayInterpreter.Interpret();
                ArrayValue = arrayInterpreter.GetArrayValue();
                Debug.Log($"Interpeted array: Array value: {ArrayValue}");
                break;
            case(TupleNode node):
                WhatExpr = WhatExpression.TupleExpr;
                var tupleInterpreter = new TupleInterpreter(node);
                tupleInterpreter.Interpret();
                TupleValue = tupleInterpreter.GetTupleValue();
                Debug.Log($"Interpeted tuple: Tuple value: {TupleValue}");
                break;
        }
    }
}