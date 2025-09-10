namespace compiler_construction.Tokenization.Types;

public class None : Token
{
    public None(string representation)
    {
        this.sourceText = representation;
    }
}
