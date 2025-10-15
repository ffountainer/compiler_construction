using compiler_construction.Syntax;
using compiler_construction.Tokenization;

namespace compiler_construction;

class SyntaxAnalyzer
{
    private Lexer _lexer;

    public SyntaxAnalyzer(Lexer lexer)
    {
        this._lexer = lexer;
    }

    public void PrintAST()
    {
        var program = NodeFactory.ConstructNode(new ProgramNode(), _lexer, _lexer.GetNextToken());
        program.PrintTree();
    }
}
