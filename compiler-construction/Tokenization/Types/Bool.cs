namespace compiler_construction.Tokenization.Types;

public class Bool : Token
{
    private bool value;
    
    public Bool(string representation)
    {
        this.sourceText = representation;
    }
    
    public bool GetValue()
    {
        return this.value;
    }
    
    public void GetValue(bool v)
    {
        this.value = v;
    }
}
