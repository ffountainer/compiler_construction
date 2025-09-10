namespace compiler_construction.Tokenization.Keywords;

public class BoolKeyword : Token
{
    public BoolKeyword(string representation)
    {
        this.sourceText = representation;
    }
}
