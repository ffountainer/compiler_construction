using System.Collections;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class ForLoopNode : TreeNode
{
    public override string GetName()
    {
        return "ForLoop";
    }
    
    public override void ReadTokens(out Token lastToken)
    {
        bool wasInner = IsInnerLoop;
        if (IsLoop || IsForLoop)
        {   
            IsInnerLoop = true;
            wasInner = true;
        }
        IsLoop = true;
        IsForLoop = true;
        Scope new_scope = new Scope(new Hashtable(), SyntaxAnalyzer.GetCurrentScope());
        SyntaxAnalyzer.SetScope(new_scope);
        children.Add(NodeFactory.ConstructNode(new ForHeader(), lexer, firstToken, out var token));
        children.Add(NodeFactory.ConstructNode(new LoopBodyNode(), lexer, token, out lastToken));
        if (!IsInnerLoop)
        {
            IsLoop = false;
            IsForLoop = false; 
        }

        if (!wasInner)
        {
            IsInnerLoop = false;
        }
        SyntaxAnalyzer.SetScope(SyntaxAnalyzer.GetParentScope());
    }
}
