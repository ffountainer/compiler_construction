namespace compiler_construction;

using Tokenization;
using Tokenization.Types;
using Tokenization.BoundingOperators;
using Tokenization.Keywords;
using Tokenization.Operators;
using Tokenization.Symbols;

class Program
{
    private static FileStream _fileStream;
    private static StreamReader _streamReader;
    
    private static void Main(string[] args)
    {
        string path = "../../../code.d";
        _fileStream =  new FileStream(path, FileMode.Open);
        _streamReader = new StreamReader(_fileStream);
        
        Lexer();
        // ..
    }
    
    private static Token Lexer()
    {
        var currentToken = "";
        var loop = true;
        while (loop)
        {
            var registeredToken = false;
            var readValue = _streamReader.Read();
            if (readValue == -1)
            {
                loop = false;
            }
            else
            {
                var character = (char)readValue;
                if (char.IsWhiteSpace(character))
                {
                    if (registeredToken)
                    {
                        loop = false;
                        currentToken = "";
                    }
                }
                else
                {
                    currentToken += character;
                    registeredToken = true;
                    switch (character)
                    {
                        // case '('
                    }
                }
            }
        }
        Console.Write(currentToken);

        return new Semicolon();
    }
}
