namespace compiler_construction;

using Tokenization;
using Tokenization.Types;
using Tokenization.BoundingOperators;
using Tokenization.Keywords;
using Tokenization.Operators;
using Tokenization.Symbols;
using System.Globalization;

class Program
{
    private static FileStream _fileStream;
    private static StreamReader _streamReader;
    private static Token _nextToken = null;
    
    private static void Main(string[] args)
    {
        string path = "../../../code.d";
        _fileStream =  new FileStream(path, FileMode.Open);
        _streamReader = new StreamReader(_fileStream);

        while (!_streamReader.EndOfStream)
        {
            Token newToken = Lexer();
            Console.WriteLine("Tk: " + newToken.GetType().Name + " | " + newToken.GetSourceText());
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
                _streamReader.Read();
            }
            else 
            {
                var extra = _streamReader.Read();
            }

        }

        string sourceText = character.ToString();

        
        if (_nextToken != null)
        {
            var retValue = _nextToken;
            _nextToken = null;
            return retValue;
        }
        
        switch (character)
            {
                case '(':
                    return new LeftBrace(sourceText);
                case ')':
                    return new RightBrace(sourceText);
                case '{':
                    return new LeftCurlyBrace(sourceText);
                case '}':
                    return new RightCurlyBrace(sourceText);
                case '[':
                    return new LeftBracket(sourceText);
                case ']':
                    return new RightBracket(sourceText);
                case '"':
                    return checkQuote();
                case '\'':
                    return checkQuote();
                case '/':
                    var readCh3 = _streamReader.Peek();
                    if (readCh3 == -1)
                    {
                        break;
                    }
                    var newCh3 = (char)readCh3;
                    if (newCh3 == '=')
                    {
                        var extra = _streamReader.Read();
                        return new NotEqual("/=");
                    }
                    return new Divide("/");
                case '=':
                    var readCh6 = _streamReader.Peek();
                    if (readCh6 == -1)
                    {
                        break;
                    }
                    var newCh6 = (char)readCh6;
                    if (newCh6 == '>')
                    {
                        var extra = _streamReader.Read();
                        return new EqualGreater("=>");
                    }
                    return new Equal("=");
                case '>':
                    var readCh = _streamReader.Peek();
                    if (readCh == -1)
                    {
                        break;
                    }
                    var newCh = (char)readCh;
                    if (newCh == '=')
                    {
                        var extra = _streamReader.Read();
                        return new GreaterEqual(">=");
                    }
                    return new Greater(">");
                case '<':
                    var readCh2 = _streamReader.Peek();
                    if (readCh2 == -1)
                    {
                        break;
                    }
                    var newCh2 = (char)readCh2;
                    if (newCh2 == '=')
                    {
                        var extra = _streamReader.Read();
                        return new LessEqual("<=");
                    }
                    return new Less("<");
                case '-':
                    return new Minus("-");
                case '+':
                    return new Plus("+");
                case '*':
                    return new Times("*");
                case ':':
                    var readCh5 = _streamReader.Peek();
                    if (readCh5 == -1)
                    {
                        break;
                    }
                    var newCh5 = (char)readCh5;
                    if (newCh5 == '=')
                    {
                        var extra = _streamReader.Read();
                        return new ColonEqual(":=");
                    }
                    break;
                case ',':
                    return new Comma(",");
                case '.':
                    var readCh7 = _streamReader.Peek();
                    if (readCh7 == -1)
                    {
                        break;
                    }
                    var newCh7 = (char)readCh7;
                    if (newCh7 == '.')
                    {
                        var extra = _streamReader.Read();
                        return new Range("..");
                    }
                    return new Point(".");
                case ';':
                    return new Semicolon(";");
                case char ch when (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'):
                    bool text = true;
                    var str = character.ToString();
                    while (text)
                    {
                        var readChar = _streamReader.Peek();
                        if (readChar == -1)
                        {
                            return checkKeyword(str);
                        }

                        var newChar = (char)readChar;

                        if (";,:=>.</([{}])".Contains(newChar))
                        { 
                            return checkKeyword(str);
                        }

                        if (char.IsWhiteSpace(newChar))
                        {
                            return checkKeyword(str);
                        }

                        str += newChar;
                        _streamReader.Read();

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

                        if (";,:=>< /}]+-*)".Contains(newChar) || readChar == -1 || char.IsWhiteSpace(newChar))
                        {
                            number = false;
                            if (isReal)
                            {
                                return new Real(value, float.Parse(value, CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                return new Int(value, Int32.Parse(value));
                            }
                        }
                        if (newChar == '.')
                        {
                            _streamReader.Read();
                            var checkIfRange = _streamReader.Peek();
                            var check = (char)checkIfRange;
                            if (check != '.')
                            {
                                isReal = true;
                                value += newChar;
                                value += check;
                                _streamReader.Read();
                                
                            }
                            else
                            {
                                _nextToken = new Range("..");
                                number = false;
                                return new Int(value, Int32.Parse(value));
                            }

                        }
                        else
                        {
                            value += newChar;
                            _streamReader.Read();
                        }
                        
                        
                        
                    }

                    break;
                    
        }

        return new FinishProgram();
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
                return new String(newChar + str + newChar, str);
            }
            str += newChar;
        }
        return new FinishProgram();
    }

    private static Token checkKeyword(string str)
    {
        switch (str) {
            case "and": return new And(str);
            case "else" : return new Else(str);
            case "end" : return new End(str);
            case "exit": return new Exit(str);
            case "for" : return new For(str);
            case "if" : return new If(str);
            case "in": return new In(str);
            case "is" : return new Is(str);
            case "loop" : return new Loop(str);
            case "not": return new Not(str);
            case "or" : return new Or(str);
            case "print" : return new Print(str);
            case "return": return new Return(str);
            case "then" : return new Then(str);
            case "var" : return new Var(str);
            case "while": return new While(str);
            case "xor" : return new Xor(str);
            case "bool" : return new BoolKeyword(str);
            case "int": return new IntKeyword(str);
            case "none" : return new NoneKeyword(str);
            case "real" : return new RealKeyword(str);
            case "string": return new StringKeyword(str);
            case "func": return new Func(str);
            default: return new Identifier(str);
        }
    }
}
