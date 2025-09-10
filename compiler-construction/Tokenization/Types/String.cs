namespace compiler_construction.Tokenization.Types;

public class String : Token
{
    private string value;
    public String(string source, string str)
    {
        this.sourceText = source;
        this.value = str;
    }
}
