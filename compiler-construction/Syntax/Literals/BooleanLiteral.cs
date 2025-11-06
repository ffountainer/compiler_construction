using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class BooleanLiteral : TreeNode
{
    public bool Value;
    
    public override string GetName()
    {
        return "Boolean" + " " + Value;
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
        if (firstToken.GetSourceText().Equals("true"))
        {
            Value = true;
        }
        else
        {
            Value = false;
        }
    }

    public BooleanLiteral WithValue(bool value)
    {
        Value = value;
        return this;
    }
}
