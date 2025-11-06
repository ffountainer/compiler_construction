using System.Collections;
using System.Text;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Types;
using String = System.String;

namespace compiler_construction.Syntax;

public abstract class TreeNode
{
    protected Lexer lexer;
    protected Token firstToken;
    protected List<TreeNode> children = new List<TreeNode>();

    protected static bool IsLoop = false;
    protected static bool IsFunc = false;
    protected static bool IsAssign = false;
    protected static bool IsForLoop = false;
    protected static bool IsWhileLoop = false;
    protected static String currentIdent = "";

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

    public List<TreeNode> GetChildren()
    {
        return children;
    }

    public void ClearChildren()
    {
        children.Clear();
    }

    public TreeNode AddChild(TreeNode child)
    {
        children.Add(child);
        return this;
    }

    public override string ToString()
    {
        var builder = new StringBuilder().Append(GetName()).Append(" { ");

        foreach (var child in children)
        {
            builder.Append(child.GetName()).Append(", ");
        }
        
        return builder.Append(" }").ToString();
    }
}
