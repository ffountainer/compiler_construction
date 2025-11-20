using compiler_construction.Syntax;
using compiler_construction.Tokenization;
using System.Collections;
using compiler_construction.Semantics;
using compiler_construction.Syntax.Literals;

namespace compiler_construction;

class SyntaxAnalyzer
{
    private Lexer _lexer;
    private ProgramNode _program;

    public static Scope currentScope = new Scope(new Hashtable(), null);
    public SyntaxAnalyzer(Lexer lexer)
    {
        this._lexer = lexer;
    }
    
    public static void AddToCurScope(String key, bool isDefined)
    {
        currentScope.AddToScope(key, isDefined);
    }
    
    public static Scope GetCurrentScope()
    {
        return currentScope;
    }

    public static Scope GetParentScope()
    {
        return currentScope.GetParentScope();
    }

    public static void SetScope(Hashtable sc, Scope parentScope)
    {
        currentScope = new Scope(sc, parentScope);
    }
    
    public static void SetScope(Scope scope)
    {
        currentScope = scope;
    }

    public void PrintAST()
    {
        var program = NodeFactory.ConstructNode(new ProgramNode(), _lexer, _lexer.GetNextToken());
        _program = program;
        // program.PrintTree();
    }

    public ProgramNode GetTree()
    {
        return _program;
    } 
}
