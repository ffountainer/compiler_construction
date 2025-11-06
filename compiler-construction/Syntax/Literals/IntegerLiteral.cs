using compiler_construction.Semantics;
using compiler_construction.Tokenization;

namespace compiler_construction.Syntax.Literals;

public class IntegerLiteral : TreeNode
{
    public int Value;
    
    public override string GetName()
    {
        return "INTEGER" + " " + Value;
    }

    public override void ReadTokens(out Token lastToken)
    {
        lastToken = firstToken;
        if (int.TryParse(firstToken.GetSourceText(), out var number))
            Value = number;
        else
            throw new SemanticException($"invalid integer literal {firstToken.GetSourceText()}");
    }

    public IntegerLiteral WithValue(int value)
    {
        Value = value;
        return this;
    }
}
