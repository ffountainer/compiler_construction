using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class NodeFactory
{
    public static T ConstructNode<T>(T node, Lexer lexer, Token firstToken) where T : TreeNode
    {
        return ConstructNode(node, lexer, firstToken, out _);
    }
    
    public static T ConstructNode<T>(T node, Lexer lexer, Token firstToken, out Token lastToken) where T : TreeNode
    {
        node.Init(lexer, firstToken);
        node.ReadTokens(out lastToken);

        if (node is ConstReduceableNode reduceable)
        {
            reduceable.Reduce();
        }
        
        return node;
    }
}
