using System.Collections;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class AssignmentNode : TreeNode
{
    public override string GetName()
    {
        return "Assignment";
    }

    public override void ReadTokens(out Token lastToken)
    {
        IsAssign = true;
        
        bool flag = false;
        var check_scope = SyntaxAnalyzer.GetCurrentScope();
        do
        {
            if (check_scope.GetScope().ContainsKey(firstToken.GetSourceText()))
            {
                flag = true;
            }

            check_scope = check_scope.GetParentScope();
        } while (check_scope != null && flag == false);
            
        if (!flag)
        {
            throw new SemanticException($"No such variable declared \"{firstToken.GetSourceText()}\"");
        }

        currentIdent = firstToken.GetSourceText();
        
        children.Add(NodeFactory.ConstructNode(new ReferenceNode(), lexer, firstToken, out var colonEqual));

        if (colonEqual is not ColonEqual)
        {
            throw new UnexpectedTokenException($"Expected :=, got {colonEqual}");
        }
        
        children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out lastToken));
        
        SyntaxAnalyzer.GetCurrentScope().GetScope()[firstToken.GetSourceText()] = true;
        IsAssign = false;
    }
}
