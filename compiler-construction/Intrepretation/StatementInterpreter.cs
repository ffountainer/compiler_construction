using compiler_construction.Syntax;

namespace compiler_construction.Intrepretation;

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
                    break;
                case(AssignmentNode assignmentNode):
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