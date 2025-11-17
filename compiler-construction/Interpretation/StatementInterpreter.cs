using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class StatementInterpreter
{
    private StatementNode _statement;
    
    public StatementInterpreter(StatementNode statement)
    {
        _statement = statement;
    }
    public void Interpret()
    {
        foreach (var child in _statement.GetChildren())
        {
            switch (child)
            {
                case(DeclarationNode declarationNode):
                    var declaration = new DeclarationInterpreter(declarationNode);
                    declaration.Interpret();
                    Debug.Log("The result of the new declaration:");
                    foreach (IdentifierNode name in Interpreter.GetIdentifiers().Keys.ToList())
                    {
                        Debug.Log($">> {name.GetValue()} + {name}");
                    }
                    break;
                case(AssignmentNode assignmentNode):
                    var assignment = new AssignmentInterpreter(assignmentNode);
                    assignment.Interpret();
                    // TODO: all assignments apart from simple idents
                    break;
                case(IfNode ifNode):
                    break;
                case(WhileLoopNode WhileLoopNode):
                    break;
                case(ForLoopNode ForLoopNode):
                    break;
                case (LoopBodyNode loopBodyNode):
                    break;
                case(ReturnNode returnNode):
                    break;
                case(ExitNode exitNode):
                    break;
                case(PrintNode printNode):
                    
                    break;
                default:
                    throw new InterpretationException("Interpreter: no valid statement detected");
            }
        }
    }
}