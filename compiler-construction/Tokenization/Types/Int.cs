namespace compiler_construction.Tokenization.Types;

public class Int : Numeric<int>
{
    private int value;
    
    public Int(int value) : base(value) { }
    
}