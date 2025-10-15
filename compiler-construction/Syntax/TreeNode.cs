using System.Collections;
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
        Console.WriteLine(new string(' ', level) + GetName());
        foreach (var child in children)
        {
            child.PrintTree(level + 1);
        }
    }
}

// ident [ a + b ]