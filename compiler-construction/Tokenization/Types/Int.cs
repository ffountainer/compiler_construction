namespace compiler_construction.Tokenization.Types;

public class Int : Numeric<int>
{
    private int value;

    public Int(string source, int value) : base(source, value) { }
}
