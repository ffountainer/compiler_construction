using compiler_construction.Interpretation;
using compiler_construction.Syntax;
namespace compiler_construction;

public class Interpreter : Interpretable
{
    private ProgramNode _program;

    public Interpreter(ProgramNode program)
    {
        _program = program;
        currentScope = new InterpretationScope(new Dictionary<IdentifierNode, ExpressionNode?>(), null);
    }

    public override void Interpret()
    {
        parentScope = null;
        int i = 0;
        foreach (StatementNode child in _program.GetChildren())
        {
            Debug.Log($"interpreting statement {i}");
            i += 1;
            var statement = new StatementInterpreter(child);
            statement.Interpret();
        }
    }
}