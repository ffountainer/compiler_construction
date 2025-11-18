using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class StatementInterpreter : Interpretable
{
    private StatementNode _statement;
    
    public StatementInterpreter(StatementNode statement)
    {
        _statement = statement;
        children = statement.GetChildren();
    }
    public override void Interpret()
    {
        foreach (var child in _statement.GetChildren())
        {
            switch (child)
            {
                case(DeclarationNode declarationNode):
                    Debug.Log("Im interpreting declaration:");
                    var declaration = new DeclarationInterpreter(declarationNode);
                    declaration.Interpret();
                    Debug.Log("The result of the new declaration:");
                    foreach (IdentifierNode name in Interpreter.GetIdentifiers().Keys.ToList())
                    {
                        Debug.Log($">> {name.GetValue()} + {name}");
                    }
                    break;
                case(AssignmentNode assignmentNode):
                    Debug.Log("Im interpreting assignment:");
                    var assignment = new AssignmentInterpreter(assignmentNode);
                    assignment.Interpret();
                    // TODO: assignment for call
                    Debug.Log("I have finished interpreting assignment");
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
                    Debug.Log("Im interpreting print");
                    var print = new PrintInterpreter(printNode);
                    print.Interpret();
                    Debug.Log("I have finished interpreting print");
                    break;
                default:
                    throw new InterpretationException("Interpreter: no valid statement detected");
            }
        }
    }
}