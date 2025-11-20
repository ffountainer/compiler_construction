using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class WhileLoopInterpreter : Interpretable
{
    private WhileLoopNode _whileLoop;

    public WhileLoopInterpreter(WhileLoopNode whileLoop)
    {
        _whileLoop = whileLoop;
        children = whileLoop.GetChildren();
    }
    public override void Interpret()
    {
        isLoop = true;
        Debug.Log("Im beginning to interpret while loop");
        ExpressionNode expression = (ExpressionNode)children.First();
        ExpressionInterpreter expressionInterpreter = new ExpressionInterpreter(expression);
        expressionInterpreter.Interpret();

        bool isSatisfied = expressionInterpreter.GetBoolValue();
        
        while (isSatisfied)
        {
            SetNewScope();
            BodyNode body = (BodyNode)children.Last().GetChildren().First();
            BodyInterpreter bodyInterpreter = new BodyInterpreter(body);
            bodyInterpreter.Interpret();
            InheritValues(bodyInterpreter, "Interpreter: error inheriting from then body while interpreting while statement");
            ExpressionInterpreter updateInterpreter = new ExpressionInterpreter(expression);
            updateInterpreter.Interpret();
            isSatisfied = updateInterpreter.GetBoolValue();
            if (bodyInterpreter.GetExitStatement()) isSatisfied = false;
            returnPrevScope();
        }

        isLoop = false;
        exitStatement = false;
    }
}