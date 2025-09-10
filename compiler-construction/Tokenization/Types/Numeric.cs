namespace compiler_construction.Tokenization.Types;

public abstract class Numeric<T> : Token
{
    protected T value;
    
    public Numeric(string representation, T givenValue)
    {
        this.value = givenValue;
        this.sourceText = representation;
    }

    public void SetValue(T givenValue)
    {   
        this.value = givenValue;
    }
    
    public T GetValue()
    {
        return this.value;
    }
}
