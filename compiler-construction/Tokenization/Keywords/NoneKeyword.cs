namespace compiler_construction.Tokenization.Keywords;

public class NoneKeyword : Token
{
    public NoneKeyword(string representation)
    {
        this.sourceText = representation;
    }
}
