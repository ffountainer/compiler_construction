using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class ForLoopInterpreter : Interpretable
{
    private ForLoopNode _forLoopNode;

    public ForLoopInterpreter(ForLoopNode forLoopNode)
    {
        _forLoopNode = forLoopNode;
        children = forLoopNode.GetChildren();
    }
    public override void Interpret()
    {
        isLoop = true;
        Debug.Log("Im beginning to interpret for loop");
        bool hasForHeader = (children.Count > 1);

        BodyNode loopBodyNode = (BodyNode)children.Last().GetChildren()[0];
        BodyInterpreter bodyInterpreter = new BodyInterpreter(loopBodyNode);

        if (hasForHeader)
        {
            SetNewScope();
            ForHeader forHeader = (ForHeader)children.First();
            List<TreeNode> forHeaderChildren = forHeader.GetChildren();
            
            if (forHeaderChildren.Count == 1)
            {
                // only one integer
                ExpressionNode bound = (ExpressionNode)forHeaderChildren[0];
                ExpressionInterpreter boundInterpreter = new ExpressionInterpreter(bound);
                boundInterpreter.Interpret();
                int boundary = boundInterpreter.GetIntValue();

                for (int i = 0; i < boundary; i++)
                {
                    bodyInterpreter.Interpret();
                }
            }
            
            if (forHeaderChildren.Count == 2)
            {
                // lower bound and higher bound
                if (forHeaderChildren is ExpressionNode)
                {
                    ExpressionNode boundLow = (ExpressionNode)forHeaderChildren[1];
                    ExpressionInterpreter boundLowInterpreter = new ExpressionInterpreter(boundLow);
                    boundLowInterpreter.Interpret();
                    int lowBoundary = boundLowInterpreter.GetIntValue();

                    ExpressionNode boundHigh = (ExpressionNode)forHeaderChildren[2];
                    ExpressionInterpreter boundHighInterpreter = new ExpressionInterpreter(boundLow);
                    boundLowInterpreter.Interpret();
                    int highBoundary = boundHighInterpreter.GetIntValue();

                    for (int i = lowBoundary; i < highBoundary; i++)
                    {
                        bodyInterpreter.Interpret();
                    }
                }
                else
                {
                    IdentifierNode forHeaderIdentifier = (IdentifierNode)forHeaderChildren[0];
                    //  higher bound
                    ExpressionNode value = ConstructLiteralExpr(0, WhatExpression.IntegerExpr);
                    AddIdentifier(forHeaderIdentifier, value);
                
                    ExpressionNode boundHigh = (ExpressionNode)forHeaderChildren[1];
                    ExpressionInterpreter boundHighInterpreter = new ExpressionInterpreter(boundHigh);
                    boundHighInterpreter.Interpret();
                    int highBoundary = boundHighInterpreter.GetIntValue();
                
                    for (int i = 0; i < highBoundary; i++)
                    {
                        bodyInterpreter.Interpret();
                        ExpressionInterpreter oldValue = new ExpressionInterpreter(value);
                        oldValue.Interpret();
                        value = ConstructLiteralExpr(oldValue.GetIntValue() + 1, WhatExpression.IntegerExpr);
                        SetIdentifier(forHeaderIdentifier, value);
                    }
                }
            }
            
            if (forHeaderChildren.Count == 3)
            {
                IdentifierNode forHeaderIdentifier = (IdentifierNode)forHeaderChildren[0];
                // lower bound and higher bound
                ExpressionNode boundLow = (ExpressionNode)forHeaderChildren[1];
                ExpressionInterpreter boundLowInterpreter = new ExpressionInterpreter(boundLow);
                boundLowInterpreter.Interpret();
                int lowBoundary = boundLowInterpreter.GetIntValue();
                
                ExpressionNode value = ConstructLiteralExpr(lowBoundary, WhatExpression.IntegerExpr);
                AddIdentifier(forHeaderIdentifier, value);
                
                ExpressionNode boundHigh = (ExpressionNode)forHeaderChildren[2];
                ExpressionInterpreter boundHighInterpreter = new ExpressionInterpreter(boundHigh);
                boundHighInterpreter.Interpret();
                int highBoundary = boundHighInterpreter.GetIntValue();
                
                for (int i = lowBoundary; i < highBoundary; i++)
                {
                    bodyInterpreter.Interpret();
                    ExpressionInterpreter oldValue = new ExpressionInterpreter(value);
                    oldValue.Interpret();
                    value = ConstructLiteralExpr(oldValue.GetIntValue() + 1, WhatExpression.IntegerExpr);
                    SetIdentifier(forHeaderIdentifier, value);
                }
            }
            
            returnPrevScope();
        }
        else
        {
            while (true)
            {
                bodyInterpreter.Interpret();
            }
        }
        
        isLoop = false;
        exitStatement = false;
    }
}