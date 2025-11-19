using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class StringLiteral : TreeNode
{
    public string Value;
    public override string GetName()
    {
        return "STRING" + " " + firstToken.GetSourceText();
    }

    public override void ReadTokens(out Token lastToken)
    {
        Value = firstToken.GetSourceText();
        lastToken = firstToken;
    }
    
    public StringLiteral WithValue(string value)
    {
        Value = value;
        return this;
    }
}
