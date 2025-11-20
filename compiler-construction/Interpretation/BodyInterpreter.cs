using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class BodyInterpreter : Interpretable
{
    private BodyNode _body;

    public BodyInterpreter(BodyNode body)
    {
        _body = body;
        children = body.GetChildren();
    }
    public override void Interpret()
    {
        List<IdentifierNode> addedKeys = new List<IdentifierNode>();
        Debug.Log("Im beginning to interpret the body");
        foreach (StatementNode child in _body.GetChildren())
        {
            var statement = new StatementInterpreter(child);
            statement.Interpret();
            if (child.GetChildren().Count > 0 && child.GetChildren().First() is DeclarationNode)
            {
               IdentifierNode newKey = (IdentifierNode)child.GetChildren().First().GetChildren()[0].GetChildren()[0]; 
               addedKeys.Add(newKey);
            }
            if (exitStatement)
            {
                break;
            }
            
            InheritValues(statement, "Interpreter: error inheriting statement values while interpreting body");
        }

        foreach (IdentifierNode key in addedKeys)
        {
            currentScope.DeleteIdentifier(key);
        }
    }
}