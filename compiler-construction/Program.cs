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

        while (!_streamReader.EndOfStream)
        {
            Console.Write(Lexer());
        }
        
        _fileStream.Close();
        _streamReader.Close();
    }
    
    private static Token Lexer()
    {
        var spaces = true;

        char character = '\0';

        while (spaces)
        {
            var readValue = _streamReader.Peek();
            if (readValue == -1)
            {
                return new FinishProgram();
            }

            character = (char)readValue;
            if (!char.IsWhiteSpace(character))
            {
                spaces = false;
            }
            else 
            {
                var extra = _streamReader.Read();
            }

        }

        switch (character)
            {
                case '(':
                    return new LeftBrace();
                case ')':
                    return new RightBrace();
                case '{':
                    return new LeftCurlyBrace();
                case '}':
                    return new RightCurlyBrace();
                case '[':
                    return new LeftBracket();
                case ']':
                    return new RightBracket();
                case '"':
                    checkQuote();
                    break;
                case '\'':
                    checkQuote();
                    break;
                case '/':
                    var readCh3 = _streamReader.Peek();
                    if (readCh3 == -1)
                    {
                        Console.Write("File ended");
                        break;
                    }
                    var newCh3 = (char)readCh3;
                    if (newCh3 == '=')
                    {
                        var extra = _streamReader.Read();
                        return new NotEqual();
                    }
                    return new Divide();
                case '=':
                    var readCh6 = _streamReader.Peek();
                    if (readCh6 == -1)
                    {
                        Console.Write("File ended");
                        break;
                    }
                    var newCh6 = (char)readCh6;
                    if (newCh6 == '>')
                    {
                        var extra = _streamReader.Read();
                        return new EqualGreater();
                    }
                    return new Equal();
                case '>':
                    var readCh = _streamReader.Peek();
                    if (readCh == -1)
                    {
                        Console.Write("File ended");
                        break;
                    }
                    var newCh = (char)readCh;
                    if (newCh == '=')
                    {
                        var extra = _streamReader.Read();
                        return new GreaterEqual();
                    }
                    return new Greater();
                case '<':
                    var readCh2 = _streamReader.Peek();
                    if (readCh2 == -1)
                    {
                        Console.Write("File ended");
                        break;
                    }
                    var newCh2 = (char)readCh2;
                    if (newCh2 == '=')
                    {
                        var extra = _streamReader.Read();
                        return new LessEqual();
                    }
                    return new Less();
                case '-':
                    return new Minus();
                case '+':
                    return new Plus();
                case '*':
                    return new Times();
                case ':':
                    var readCh5 = _streamReader.Peek();
                    if (readCh5 == -1)
                    {
                        Console.Write("File ended");
                        break;
                    }
                    var newCh5 = (char)readCh5;
                    if (newCh5 == '=')
                    {
                        var extra = _streamReader.Read();
                        return new ColonEqual();
                    }
                    Console.Write("Weird :");
                    break;
                case ',':
                    return new Comma();
                case '.':
                    var readCh7 = _streamReader.Peek();
                    if (readCh7 == -1)
                    {
                        Console.Write("File ended");
                        break;
                    }
                    var newCh7 = (char)readCh7;
                    if (newCh7 == '.')
                    {
                        var extra = _streamReader.Read();
                        return new Range();
                    }
                    return new Point();
                case ';':
                    return new Semicolon();
                case char ch when (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'):
                    bool text = true;
                    var str = character.ToString();
                    while (text)
                    {
                        var readChar = _streamReader.Peek();
                        if (readChar == -1)
                        {
                            Console.Write("File ended");
                            return checkKeyword(str);
                        }

                        var newChar = (char)readChar;

                        if (";.,:=>< /}])".Contains(newChar))
                        { 
                            return checkKeyword(str);
                        }

                        if (char.IsWhiteSpace(newChar))
                        {
                            return checkKeyword(str);
                        }

                        str += newChar;

                    }

                    break;
                case char ch when (ch >= '0' && ch <= '9'):
                    bool number = true;
                    var value = character.ToString();
                    bool isReal = false;
                    while (number)
                    {
                        var readChar = _streamReader.Peek();
                        
                        var newChar = (char)readChar;

                        if (";,:=>< /}])".Contains(newChar) || readChar == -1 || char.IsWhiteSpace(newChar))
                        {
                            number = false;
                            if (isReal)
                            {
                                return new Real(float.Parse(value));
                            }
                            else
                            {
                                return new Int(Int32.Parse(value));
                            }
                        }

                        if (newChar == '.')
                        {
                            isReal = true;
                            value += newChar;
                        }
                        var extra = _streamReader.Read();
                        value += newChar;
                    }

                    break;
                    
        }

        return new Semicolon();
    }

    private static Token checkQuote()
    {
        bool quote_not_finished = true;
        var str = "";
        while (quote_not_finished)
        {
            var readChar = _streamReader.Read();
            if (readChar == -1)
            {
                Console.Write("Error: unclosed string");
                return new FinishProgram();
            }
            var newChar = (char)readChar;
            if (newChar == '"' || newChar == '\'')
            {
                quote_not_finished = false;
                return new String(str);
            }
            str += newChar;
        }
        return new FinishProgram();
    }

    private static Token checkKeyword(string str)
    {
        switch (str) {
            case "and": return new And();
            case "else" : return new Else();
            case "end" : return new End();
            case "exit": return new Exit();
            case "for" : return new For();
            case "if" : return new If();
            case "in": return new In();
            case "is" : return new Is();
            case "loop" : return new Loop();
            case "not": return new Not();
            case "or" : return new Or();
            case "print" : return new Print();
            case "return": return new Return();
            case "then" : return new Then();
            case "var" : return new Var();
            case "while": return new While();
            case "xor" : return new Xor();
            default: return new Identifier();
        }
    }
}
