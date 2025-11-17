using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;

namespace compiler_construction.Syntax;

public class ArrayAccessNode : TreeNode
{
    private ReferenceNode reference;
    
    private ExpressionNode arrayIndex;
    
    public ExpressionNode GetArrayIndex() => arrayIndex;
    
    public override string GetName()
    {
        return "ArrayElement";
    }

    public override void ReadTokens(out Token lastToken)
    {
        var node = NodeFactory
            .ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out var terminator);
        
        children.Add(node);
        arrayIndex = node;
        

        if (terminator is not RightBracket)
        {
            throw new UnexpectedTokenException($"Expected ], but got {terminator.GetSourceText()}");
        }
        
        lastToken = lexer.GetNextToken();
    }
}