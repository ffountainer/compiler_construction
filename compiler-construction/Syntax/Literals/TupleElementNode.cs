using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax.Literals;

/// <summary>
/// Puts comma or closing bracket as last token
/// </summary>
public class TupleElementNode : TreeNode
{
    public override string GetName()
    {
        return "TupleElement";
    }

    public IdentifierNode? key;
    public ExpressionNode value;
    
    public TupleElementNode() { }
    public TupleElementNode(IdentifierNode? key, ExpressionNode value, List<TreeNode> ch)
    {
        this.key = key;
        this.value = value;
        children = ch;
    }
    public override void ReadTokens(out Token lastToken)
    {
        var node = NodeFactory.ConstructNode(new ExpressionNode(calledByForHeader:true), lexer, firstToken, out var colonEqual);
        if (colonEqual is ColonEqual)
        {
            var ident = NodeFactory.ConstructNode(new IdentifierNode(), lexer, firstToken);
            key = ident;
            children.Add(ident);
            var expr = NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out lastToken);
            value = expr;
            children.Add(expr);
        }
        else
        {
            children.Add(node);
            value = node;
            lastToken = colonEqual;
        }
    }
}
