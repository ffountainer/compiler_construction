using compiler_construction.Intrepretation;
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
        foreach (StatementNode child in _program.GetChildren())
        {
            var statement = new StatementInterpreter(child);
            statement.Interpret();
        }
    }
}