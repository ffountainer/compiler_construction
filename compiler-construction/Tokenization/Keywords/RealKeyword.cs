namespace compiler_construction.Tokenization.Keywords;

public class RealKeyword : Token
{
    public RealKeyword(string representation)
    {
        this.sourceText = representation;
    }
}
