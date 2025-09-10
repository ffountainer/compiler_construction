namespace compiler_construction.Tokenization;

public class Identifier : Token
{
    public Identifier(string representation)
    {
        this.sourceText = representation;
    }
}
