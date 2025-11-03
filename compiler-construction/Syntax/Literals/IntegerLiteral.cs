using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class IntegerLiteral : TreeNode
{
    public override string GetName()
    {
        return "INTEGER" +  " " + firstToken.GetSourceText();
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
    }
}
