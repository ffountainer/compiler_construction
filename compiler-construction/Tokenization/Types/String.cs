namespace compiler_construction.Tokenization.Types;

public class String : Token
{
    private string value;
    public String(string str)
    {
        this.value = str;
    }
}