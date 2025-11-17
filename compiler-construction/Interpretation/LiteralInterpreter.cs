using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class LiteralInterpreter : Interpretable
{
    private LiteralNode _literalNode;
    
    public LiteralInterpreter(LiteralNode literalNode)
    {
        _literalNode = literalNode;
    }

    public override void Interpret()
    {
        var firstChild = _literalNode.GetChildren().First();
        switch (firstChild)
        {
            case(IntegerLiteral node):
                IntValue = node.Value;
                WhatExpr = WhatExpression.IntegerExpr;
                break;
            case(RealLiteral node):
                RealValue = node.Value;
                WhatExpr = WhatExpression.RealExpr;
                break;
            case(BooleanLiteral node):
                BoolValue = node.Value;
                WhatExpr = WhatExpression.BoolExpr;
                break;
            case(StringLiteral node):
                StringValue = node.Value;
                WhatExpr = WhatExpression.StringExpr;
                break;
            case(NoneLiteral):
                WhatExpr = WhatExpression.NoneExpr;
                break;
            case(ArrayNode node):
                WhatExpr = WhatExpression.ArrayExpr;
                var arrayInterpreter = new ArrayInterpreter(node);
                arrayInterpreter.Interpret();
                ArrayValue = arrayInterpreter.GetArrayValue();
                break;
            case(TupleNode node):
                WhatExpr = WhatExpression.TupleExpr;
                var tupleInterpreter = new TupleInterpreter(node);
                tupleInterpreter.Interpret();
                TupleValue = tupleInterpreter.GetTupleValue();
                break;
        }
    }
}