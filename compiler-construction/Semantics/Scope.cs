using System.Collections;

namespace compiler_construction.Semantics;

public class Scope
{
    private Hashtable scope = new Hashtable();
    private Scope parentScope;

    public Scope(Hashtable currentScope, Scope? parentScope)
    {
        this.scope = currentScope;
        this.parentScope = parentScope;
    }
    
    public void AddToScope(String key, bool isDefined)
    {
        scope.Add(key, isDefined);
    }
    
    public Hashtable GetScope()
    {
        return scope;
    }

    public Scope? GetParentScope()
    {
        return parentScope;
    }
    
}