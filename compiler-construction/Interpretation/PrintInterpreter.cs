using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class PrintInterpreter : Interpretable
{
    private PrintNode _printNode;

    public PrintInterpreter(PrintNode printNode)
    {
        _printNode = printNode;
        children = printNode.GetChildren();
    }

    public override void Interpret()
    {
        foreach (ExpressionNode node in _printNode.GetChildren())
        {
            ExpressionInterpreter exprInterpreter = new ExpressionInterpreter(node);
            exprInterpreter.Interpret();
            Debug.Log("Finished interpreting expression for the print, now will start printing");
            exprInterpreter.PrintExpression();
        }
        Console.WriteLine();
        
    }
}