namespace compiler_construction.Tokenization;

public abstract class Token
{
    protected string sourceText;
    
    public string GetSourceText() => sourceText;
}
