using compiler_construction.Syntax;

namespace compiler_construction.Intrepretation;

public class DeclarationInterpreter
{
    private DeclarationNode _declaration;

    public DeclarationInterpreter(DeclarationNode declaration)
    {
        _declaration = declaration;
    }

    public void Interpret()
    {
        foreach (VariableDefinitionNode child in _declaration.GetChildren())
        {
            var definition = new VarDefInterpreter(child);
            definition.Interpret();
        }
    }
}