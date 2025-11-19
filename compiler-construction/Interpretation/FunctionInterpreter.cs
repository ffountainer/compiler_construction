using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class FunctionInterpreter : Interpretable
{
    private List<TreeNode> _callArguments;

    public FunctionInterpreter(List<StatementNode> body, List<IdentifierNode> arguments,
        WhatFunction what, ExpressionNode shortExpr, List<TreeNode> givenArguments)
    {
        _callArguments = givenArguments;
        Body = body;
        Arguments = arguments;
        WhatFunc = what;
        shortFuncExpr = shortExpr;
    }
    public override void Interpret()
    {
        SetNewScope();
        int i = 0;
        if (Arguments.Count != _callArguments.Count)
        {
            throw new InterpretationException("Number of given arguments does not match number of arguments required by the function definition");
        }
        foreach (IdentifierNode ident in Arguments)
        {
            AddIdentifier(ident, (ExpressionNode)_callArguments[i]);
            i++;
        }

        if (WhatFunc is WhatFunction.Full)
        {
            foreach (StatementNode statement in Body)
            {
                StatementInterpreter statementInterpreter = new StatementInterpreter(statement);
                statementInterpreter.Interpret();
                if (statement.GetChildren().Count > 0 && statement.GetChildren().First() is ReturnNode)
                {
                    ExpressionInterpreter returnInterpreter = new ExpressionInterpreter(returnValue);
                    returnInterpreter.Interpret();
                    InheritValues(returnInterpreter, "Error inheriting from function result");
                    break;
                }
            }
        }
        else
        {
            ExpressionInterpreter expressionInterpreter = new ExpressionInterpreter(shortFuncExpr);
            expressionInterpreter.Interpret();
            InheritValues(expressionInterpreter, "Error inheriting from function result");
        }

        returnValue = null;
        returnStatement = false;
        returnPrevScope();
    }
}