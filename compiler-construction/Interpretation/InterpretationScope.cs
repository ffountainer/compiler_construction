using compiler_construction.Syntax;

namespace compiler_construction.Interpretation;

public class InterpretationScope
{
    private Dictionary<IdentifierNode, ExpressionNode?> currentScope = new Dictionary<IdentifierNode, ExpressionNode>();
    private InterpretationScope parentScope;

    public InterpretationScope(Dictionary<IdentifierNode, ExpressionNode?> newScope,
        InterpretationScope parent)
    {
        currentScope = newScope;
        parentScope = parent;
    }

    public Dictionary<IdentifierNode, ExpressionNode?> GetCurrentScope()
    {
        return currentScope;
    }

    public InterpretationScope GetParentScope()
    {
        return parentScope;
    }

    public void AddIdentifier(IdentifierNode identifier, ExpressionNode expression = null)
    {
        currentScope.Add(identifier, expression);
    }

    public void DeleteIdentifier(IdentifierNode identifier)
    {
        currentScope.Remove(identifier);
    }
    
    public void SetIdentifier(IdentifierNode identifier, ExpressionNode expression)
    {
        currentScope[identifier] = expression;
    }
    
    
}