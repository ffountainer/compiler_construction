using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class BodyInterpreter : Interpretable
{
    private BodyNode _body;

    public BodyInterpreter(BodyNode body)
    {
        _body = body;
        children = body.GetChildren();
    }
    public override void Interpret()
    {
        Debug.Log("Im beginning to interpret the body");
        foreach (StatementNode child in _body.GetChildren())
        {
            var statement = new StatementInterpreter(child);
            statement.Interpret();
            InheritValues(statement, "Interpreter: error inheriting statement values while interpreting body");
        }
    }
}