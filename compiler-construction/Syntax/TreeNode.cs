using System.Collections;
using System.Text;
using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public abstract class TreeNode
{
    protected Lexer lexer;
    protected Token firstToken;
    protected List<TreeNode> children = new List<TreeNode>();

    

    public virtual void Init(Lexer lexer, Token firstToken)
    {
        this.lexer = lexer;
        this.firstToken = firstToken;
    }
    
    public abstract void ReadTokens(out Token lastToken);

    public abstract string GetName();
    
    public virtual void PrintTree(int level = 0)
    {
        Console.WriteLine(string.Concat(Enumerable.Repeat("|  ", level)) + GetName());
        foreach (var child in children)
        {
            child.PrintTree(level + 1);
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder().Append("{ ");

        foreach (var child in children)
        {
            builder.Append(child.GetName()).Append(", ");
        }
        
        return builder.Append(" }").ToString();
    }
}
