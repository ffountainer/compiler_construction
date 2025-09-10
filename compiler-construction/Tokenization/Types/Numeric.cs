namespace compiler_construction.Tokenization.Types;

public abstract class Numeric<T> : Token
{
    private T value;
    
    public Numeric(T givenValue)
    {
        this.value = givenValue;
    }

    protected Numeric()
    {
        this.value = default(T);
    }

    public void SetValue(T givenValue)
    {   
        this.value = givenValue;
    }
    
    public T getValue()
    {
        return this.value;
    }
}