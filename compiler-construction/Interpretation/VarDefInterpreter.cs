using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class VarDefInterpreter : Interpretable
{
    private VariableDefinitionNode _definition;

    public VarDefInterpreter(VariableDefinitionNode definition)
    {
        _definition = definition;
        children = definition.GetChildren();
    }

    public override void Interpret()
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