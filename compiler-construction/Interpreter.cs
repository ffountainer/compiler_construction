using compiler_construction.Interpretation;
using compiler_construction.Syntax;
namespace compiler_construction;

public class Interpreter
{
    private ProgramNode _program;
    
    private static Dictionary<IdentifierNode, ExpressionNode?> identifiers = new Dictionary<IdentifierNode, ExpressionNode>();

    public static void AddIdentifier(IdentifierNode identifier, ExpressionNode expression = null)
    {
        identifiers.Add(identifier, expression);
    }

    public static void SetIdentifier(IdentifierNode identifier, ExpressionNode expression)
    {
        identifiers[identifier] = expression;
    }

    public static Dictionary<IdentifierNode, ExpressionNode?> GetIdentifiers()
    {
        return identifiers;
    }

    public Interpreter(ProgramNode program)
    {
        _program = program;
    }

    public void Interpret()
    {
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