namespace compiler_construction.Tokenization.Types;

public class Real : Numeric<float>
{
    private float value;
    
    public Real(float value) : base(value) { }
    
}