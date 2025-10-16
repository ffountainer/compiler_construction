using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

/// <summary>
/// Constructs Reference AST Node, puts closing operation token or the identifier itself
/// </summary>
public class ReferenceNode : TreeNode
{
    private bool calledByForHeader;

    public ReferenceNode(bool calledByForHeader)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Reference";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is not Identifier)
        {
            throw new UnexpectedTokenException("Expected identifier but got " + firstToken);
        }

        var opToken = lexer.GetNextToken();
        
        if (opToken is In)
        {
            if (calledByForHeader)
            {
                lastToken = opToken;
                return;
            }
            
            throw new UnexpectedTokenException("Expected Reference op-token but got " + opToken);
        }
        
        if (opToken is LeftBracket)
        {
            children.Add(NodeFactory.ConstructNode(new ArrayElementNode(), lexer, opToken, out lastToken));
        }
        else if (opToken is LeftBrace)
        {
            children.Add(NodeFactory.ConstructNode(new FunctionCallNode(), lexer, opToken, out lastToken));
        }
        else if (opToken is Point)
        {
            children.Add(NodeFactory.ConstructNode(new TupleAccessNode(), lexer, opToken, out lastToken));
        }
        else
        {
            children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, firstToken));
            lastToken = opToken;
        }
    }
}
