using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class NodeFactory
{
    public static T ConstructNode<T>(T node, Lexer lexer, Token firstToken) where T : TreeNode
    {
        node.Init(lexer, firstToken);
        node.ReadTokens();
        
        return node;
    }
}