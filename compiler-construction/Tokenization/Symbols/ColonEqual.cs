namespace compiler_construction.Tokenization.Symbols;

// := 

public class ColonEqual : Token
{
    public ColonEqual(string representation)
    {
        this.sourceText = representation;
    }
}
