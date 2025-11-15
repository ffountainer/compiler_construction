using compiler_construction.Syntax;

namespace compiler_construction.Intrepretation;

public class VarDefInterpreter
{
    private VariableDefinitionNode _definition;

    public VarDefInterpreter(VariableDefinitionNode definition)
    {
        this._definition = definition;
    }

    public void Interpret()
    {
        
        IdentifierNode identifier = (IdentifierNode)_definition.GetChildren().First();

        ExpressionNode expression = null;
        if (_definition.GetChildren().Count() > 1)
        {
            expression = (ExpressionNode)_definition.GetChildren().Skip(1).First();
        }
        
        if (expression != null) Interpreter.AddIdentifier(identifier, expression);
        else Interpreter.AddIdentifier(identifier);
        
    }
}