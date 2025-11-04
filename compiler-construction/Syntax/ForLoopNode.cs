using System.Collections;
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
        IsLoop = true;
        Hashtable scope = new Hashtable();
        SyntaxAnalyzer.SetScope(scope);
        children.Add(NodeFactory.ConstructNode(new ForHeader(), lexer, firstToken, out var token));
        children.Add(NodeFactory.ConstructNode(new LoopBodyNode(), lexer, token, out lastToken));
        IsLoop = false;
        SyntaxAnalyzer.SetScope(SyntaxAnalyzer.GetGlobalReferences());
    }
}
