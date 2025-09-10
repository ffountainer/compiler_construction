namespace compiler_construction.Tokenization.Types;

public class Real : Numeric<float>
{
    public Real(string source, float value) : base(source, value) { }
}
