using compiler_construction.Tokenization;
using compiler_construction.Tokenization.BoundingOperators;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class ReferenceNode : TreeNode
{
    private bool calledByForHeader;

    public ReferenceNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Reference";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        Debug.Log($"Start constructing reference from {firstToken.GetSourceText()}");
        
        if (firstToken is not Identifier)
        {
            throw new UnexpectedTokenException("Expected identifier but got " + firstToken);
        }

        var opToken = lexer.GetNextToken();
        Debug.Log($"Reference got op token {opToken.GetSourceText()}");
        
        if (opToken is In || opToken is ColonEqual)
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
            Debug.Log($"Constructed simple ident-ref, returning {lastToken.GetSourceText()} as last token");
        }
    }
}
