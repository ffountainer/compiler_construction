using compiler_construction.Syntax;

namespace compiler_construction.Intrepretation;

public class PrintInterpreter
{
    private PrintNode _printNode;

    public PrintInterpreter(PrintNode printNode)
    {
        _printNode = printNode;
    }

    public void Interpret()
    {
        foreach (ExpressionNode node in _printNode.GetChildren())
        {
            ExpressionInterpreter exprInterpreter = new ExpressionInterpreter(node);
            exprInterpreter.Interpret();
            exprInterpreter.PrintExpression();
        }
        
    }
}