using compiler_construction.Syntax;
using compiler_construction.Tokenization;
using System.Collections;
using compiler_construction.Syntax.Literals;

namespace compiler_construction;

class SyntaxAnalyzer
{
    private Lexer _lexer;
    private static Hashtable globalReferences = new Hashtable();

    public SyntaxAnalyzer(Lexer lexer)
    {
        this._lexer = lexer;
    }
    
    public static Hashtable scope = globalReferences;

    public static void AddToScope(String key, bool isDefined)
    {
        scope.Add(key, isDefined);
    }

    public static Hashtable GetScope()
    {
        return scope;
    }

    public static Hashtable GetGlobalReferences()
    {
        return globalReferences;
    }

    public static void SetScope(Hashtable sc)
    {
        scope = sc;
    }

    public void PrintAST()
    {
        var program = NodeFactory.ConstructNode(new ProgramNode(), _lexer, _lexer.GetNextToken());
        program.PrintTree();
    }
}
