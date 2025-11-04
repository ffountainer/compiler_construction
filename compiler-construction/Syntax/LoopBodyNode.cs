using System.Collections;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;

namespace compiler_construction.Syntax;

public class LoopBodyNode : TreeNode
{
    public override string GetName()
    {
        return  "LoopBody";
    }

    public override void ReadTokens(out Token lastToken)
    {
        IsLoop = true;
        if (!IsForLoop && !IsWhileLoop)
        {
            Scope new_scope = new Scope(new Hashtable(), SyntaxAnalyzer.GetCurrentScope());
            SyntaxAnalyzer.SetScope(new_scope);
        }
        children.Add(NodeFactory.ConstructNode(new BodyNode(), lexer, lexer.GetNextToken(), out lastToken));

        if (lastToken is not End)
        {
            throw new UnexpectedTokenException($"Expected loop body end, got {lastToken}");
        }
        
        lastToken = lexer.GetNextToken();
        Debug.Log("Done constructing loop body");
        IsLoop = false;
        if (!IsForLoop && !IsWhileLoop)
        {
            SyntaxAnalyzer.SetScope(SyntaxAnalyzer.GetCurrentScope().GetParentScope());
        }
    }
}
