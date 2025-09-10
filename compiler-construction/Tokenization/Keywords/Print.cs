namespace compiler_construction.Tokenization.Keywords;

public class Print : Token
{
    public Print(string representation)
    {
        this.sourceText = representation;
    }
}
