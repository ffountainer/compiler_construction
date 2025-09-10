namespace compiler_construction.Tokenization.Types;

public class Bool : Token
{
    private bool value;

    public Bool(bool value)
    {
        this.value = value;
    }
    
    public bool getValue()
    {
        return this.value;
    }
    
    public void setValue(bool v)
    {
        this.value = v;
    }
}