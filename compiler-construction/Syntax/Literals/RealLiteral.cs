using compiler_construction.Semantics;
using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class RealLiteral : TreeNode
{
    public double Value;
    
    public override string GetName()
    {
        return "REAL" + " " + Value;
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
        if (double.TryParse(firstToken.GetSourceText(), out var number))
            Value = number;
        else
            throw new SemanticException($"invalid real literal {firstToken.GetSourceText()}");
    }

    public RealLiteral WithValue(double value)
    {
        Value = value;
        return this;
    }
}
