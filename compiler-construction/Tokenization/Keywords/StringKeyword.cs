namespace compiler_construction.Tokenization.Keywords;

public class StringKeyword : Token
{
    public StringKeyword(string representation)
    {
        this.sourceText = representation;
    }
}
