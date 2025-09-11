namespace compiler_construction;

using Tokenization;

class Program
{
    private static FileStream _fileStream = null;
    private static StreamReader _streamReader = null;
    
    private static void Main(string[] args)
    {
        string path = "../../../tests/tuples/test4.d";
        _fileStream =  new FileStream(path, FileMode.Open);
        _streamReader = new StreamReader(_fileStream);
        Lexer lexer = new Lexer(path, _streamReader);

        while (!_streamReader.EndOfStream)
        {
            Token newToken = lexer.GetNextToken();
            Console.WriteLine("Tk: " + newToken.GetType().Name + " | \"" + newToken.GetSourceText() + "\"");
        }
        
        _fileStream.Close();
        _streamReader.Close();
    }
    
}
