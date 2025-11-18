using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Operators;

namespace compiler_construction.Syntax;

public class RelationNode : ConstReduceableNode
{
    private bool calledByForHeader;

    private ConstReduceableNode lhs;
    private Token? theOperator;
    private ConstReduceableNode? rhs;
    
    public ConstReduceableNode GetLHS() => lhs;
    public ConstReduceableNode? GetRHS() => rhs;
    
    public Token? GetTheOperator() => theOperator;

    public RelationNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Relation";
    }

    public override void ReadTokens(out Token lastToken)
    {
        lhs = NodeFactory.ConstructNode(new FactorNode(calledByForHeader), lexer, firstToken, out var opToken);
        children.Add(lhs);
        IsConst = lhs.IsConst;

        if (AcceptableOperation(opToken))
        {
            theOperator = opToken;
            rhs = NodeFactory.ConstructNode(new FactorNode(), lexer, lexer.GetNextToken(), out lastToken);
            children.Add(rhs);

            IsConst = lhs.IsConst && rhs.IsConst;
        }
        else
        {
            lastToken = opToken;
        }
        
        Debug.Log($"Relation node returns {lastToken.GetSourceText()} as last token");
    }

    private bool AcceptableOperation(Token token)
    {
        return token is Greater || token is GreaterEqual
            || token is Less || token is LessEqual
            || token is Equal || token is NotEqual;
    }

    protected override void Calculate()
    {
        if (theOperator == null)
        {
            ValueType = lhs.GetValueType();
            
            if (ValueType == ConstValueType.Boolean) BoolValue = lhs.GetBoolValue();
            else if (ValueType == ConstValueType.Int) IntValue = lhs.GetIntValue();
            else RealValue = lhs.GetRealValue();
            
            return;
        }
        
        ValueType = ConstValueType.Boolean;

        if (theOperator is Equal || theOperator is NotEqual)
        {
            if (lhs.GetValueType() == ConstValueType.Boolean || rhs.GetValueType() == ConstValueType.Boolean)
            {
                var equal = lhs.GetValueType() == rhs.GetValueType() && lhs.GetBoolValue() == rhs.GetBoolValue();
                BoolValue = theOperator is Equal ?  equal : !equal;
            }
            else
            {
                var equal = lhs.GetNumericalValue() == rhs.GetNumericalValue();
                BoolValue = theOperator is Equal ?  equal : !equal;
            }
            
            return;
        }

        if (lhs.GetValueType() == ConstValueType.Boolean || rhs.GetValueType() == ConstValueType.Boolean)
        {
            throw new Exception("Cannot compare boolean values as numbers");
        }

        var lhsValue = lhs.GetNumericalValue();
        var rhsValue = rhs.GetNumericalValue();
        
        if (theOperator is Greater) BoolValue =  lhsValue > rhsValue;
        else if (theOperator is Less) BoolValue = lhsValue < rhsValue;
        else if (theOperator is LessEqual) BoolValue = lhsValue <= rhsValue;
        else if (theOperator is GreaterEqual) BoolValue = lhsValue >= rhsValue;
    }
}
