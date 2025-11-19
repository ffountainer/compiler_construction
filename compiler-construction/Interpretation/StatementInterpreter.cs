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
                    foreach (IdentifierNode name in GetIdentifiers().Keys.ToList())
                    {
                        Debug.Log($">> {name.GetValue()} + {name}");
                    }
                    InheritValues(declaration, "Interpreter: error inheriting from the declaration while interpreting statement");
                    break;
                case(AssignmentNode assignmentNode):
                    Debug.Log("Im interpreting assignment:");
                    var assignment = new AssignmentInterpreter(assignmentNode);
                    assignment.Interpret();
                    // TODO: assignment for call
                    InheritValues(assignment, "Interpreter: error inheriting from the assignment while interpreting statement");
                    Debug.Log("I have finished interpreting assignment");
                    break;
                case(IfNode ifNode):
                    Debug.Log("Im interpreting if statement:");
                    var ifStatement = new IfStatementInterpreter(ifNode);
                    ifStatement.Interpret();
                    InheritValues(ifStatement, "Interpreter: error inheriting from the ifStatement while interpreting statement");
                    break;
                case(WhileLoopNode WhileLoopNode):
                    Debug.Log("Im interpreting while loop:");
                    var whileLoop = new WhileLoopInterpreter(WhileLoopNode);
                    whileLoop.Interpret();
                    InheritValues(whileLoop, "Interpreter: error inheriting from the while loop while interpreting statement");
                    break;
                case(ForLoopNode ForLoopNode):
                    Debug.Log("Im interpreting for loop:");
                    var forLoop = new ForLoopInterpreter(ForLoopNode);
                    forLoop.Interpret();
                    break;
                case (LoopBodyNode loopBodyNode):
                    break;
                case(ReturnNode returnNode):
                    break;
                case(ExitNode exitNode):
                    exitStatement = true;
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