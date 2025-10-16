using compiler_construction.Syntax;

namespace compiler_construction;

using Tokenization;

class Program
{
    private static FileStream _fileStream = null;
    private static StreamReader _streamReader = null;
    
    private static void Main(string[] args)
    {
        string path = "../../../tests/tuples/test4.d";

        Debug.Log("Hello World!");

        LexerShowcase(path);
        SyntaxAnalyzerShowcase(path);
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
}
