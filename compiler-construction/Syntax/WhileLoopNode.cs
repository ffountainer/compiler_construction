using System.Collections;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class WhileLoopNode : TreeNode
{
    private bool conditionAlwaysFalse;
    
    public bool IsConditionAlwaysFalse() => conditionAlwaysFalse;
    
    public override string GetName()
    {
        return "WhileLoop";
    }

    public override void ReadTokens(out Token lastToken)
    {
        // зондре дабалатория
        
        IsLoop = true;
        IsWhileLoop = true;
        Scope new_scope = new Scope(new Hashtable(), SyntaxAnalyzer.GetCurrentScope());
        SyntaxAnalyzer.SetScope(new_scope);
        
        var token = lexer.GetNextToken();
        
        var expressionNode =
            NodeFactory.ConstructNode(new ExpressionNode(), lexer, token, out var bodyStart);
        
        children.Add(expressionNode);
        
        children.Add(NodeFactory.ConstructNode(new LoopBodyNode(), lexer, bodyStart, out lastToken));
        
        IsLoop = false;
        IsWhileLoop = false;
        SyntaxAnalyzer.SetScope(SyntaxAnalyzer.GetCurrentScope().GetParentScope());

        if (expressionNode.IsConst)
        {
            if (expressionNode.GetValueType() == ConstValueType.Boolean && !expressionNode.GetBoolValue()
                || expressionNode.GetValueType() != ConstValueType.Boolean && expressionNode.GetNumericalValue() == 0)
            {
                conditionAlwaysFalse = true;
            }
        }
    }
}
