using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class IfStatementInterpreter : Interpretable
{
    private IfNode _ifNode;
    public IfStatementInterpreter(IfNode ifNode)
    {
        _ifNode = ifNode;
        children = ifNode.GetChildren();
    }
    public override void Interpret()
    {
        ExpressionNode condition = (ExpressionNode)_ifNode.GetChildren().First();
        ExpressionInterpreter conditionInterpreter = new ExpressionInterpreter(condition);
        conditionInterpreter.Interpret();
        bool isSatisfied = conditionInterpreter.GetBoolValue();
        
        BodyNode then = (BodyNode)_ifNode.GetChildren().Skip(1).First();
        BodyInterpreter thenInterpreter = new BodyInterpreter(then);
        
        BodyInterpreter elseBodyInterpreter = null;
        
        if (_ifNode.GetChildren().Count() == 3)
        {
            // then there is an else statement
            BodyNode elseBody = (BodyNode)_ifNode.GetChildren().Skip(2).First();
            elseBodyInterpreter = new BodyInterpreter(elseBody);
        }

        if (isSatisfied)
        {
            thenInterpreter.Interpret();
            InheritValues(thenInterpreter, "Interpreter: error inheriting from then body while interpreting if statement");
        }
        else if (elseBodyInterpreter != null)
        {
            elseBodyInterpreter.Interpret();
            InheritValues(elseBodyInterpreter, "Interpreter: error inheriting from else body while interpreting if statement");
        }
        
    }
}