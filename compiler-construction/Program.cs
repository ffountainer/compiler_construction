using compiler_construction.Syntax;

namespace compiler_construction;

using Tokenization;

class Program
{
    private static FileStream _fileStream = null;
    private static StreamReader _streamReader = null;
    
    private static void Main(string[] args)
    {
        // tests review
        // declarations/assignments/prints: 1, 2, 3, 4
        // concatenation: 5
        // if statements: 6
        // loops: 7, 8, 9, 12
        // functions: 10, 11
        string path = "../../../tests/interpretation/test1.d";
        // string path = "../../../tests/semantics/scopes/test6.d";
        Debug.Log("Hello World!");

        // LexerShowcase(path);
        // SyntaxAnalyzerShowcase(path);
        InterpreterShowcase(path);
    }

    private static void LexerShowcase(string path)
    {
        var fileStream =  new FileStream(path, FileMode.Open);
        var streamReader = new StreamReader(fileStream);
        Lexer lexer = new Lexer(path, streamReader);

        while (!streamReader.EndOfStream)
        {
            Token newToken = lexer.GetNextToken();
            Console.WriteLine("Tk: " + newToken.GetType().Name + " | \"" + newToken.GetSourceText() + "\"");
        }
        
        fileStream.Close();
        streamReader.Close();
    }

    private static void SyntaxAnalyzerShowcase(string path)
    {
        var filestream = new FileStream(path, FileMode.Open);
        var streamReader = new StreamReader(filestream);
        
        var lexer = new Lexer(path, streamReader);
        var analyzer = new SyntaxAnalyzer(lexer);
        analyzer.PrintAST();
        
        streamReader.Close();
        filestream.Close();
    }

    private static void InterpreterShowcase(string path)
    {
        var filestream = new FileStream(path, FileMode.Open);
        var streamReader = new StreamReader(filestream);
        
        var lexer = new Lexer(path, streamReader);
        var analyzer = new SyntaxAnalyzer(lexer);
        analyzer.PrintAST();

        Console.WriteLine("Interpretation:");
        var interpreter = new Interpreter(analyzer.GetTree());
        interpreter.Interpret();
        
        streamReader.Close();
        filestream.Close();
    }
}
