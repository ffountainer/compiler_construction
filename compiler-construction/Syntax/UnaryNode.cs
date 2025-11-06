using compiler_construction.Semantics;
using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Operators;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

/// <summary>
/// Should place following token as last token
/// </summary>
public class UnaryNode : ConstReduceableNode
{
    // LATER: Add storage for other 2 cases
    private Token? primaryOperator;
    private PrimaryNode primaryOperand;
    
    private bool calledByForHeader;
    
    public UnaryNode(bool calledByForHeader = false)
    {
        this.calledByForHeader = calledByForHeader;
    }
    
    public override string GetName()
    {
        return "Unary";
    }

    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is not Identifier || IsPrimaryOperator(firstToken))
        {
            Debug.Log($"Unary goes to construct Primary " +
                      $"because [ ident: {firstToken is Identifier}, primary op: {IsPrimaryOperator(firstToken)} ], " +
                      $"where firstToken is {firstToken.GetSourceText()}");
            
            var token = firstToken;
            if (IsPrimaryOperator(token))
            {
                primaryOperator = token;
                Debug.Log("Semantic", $"Unary encountered primary operator {primaryOperator.GetSourceText()}");
                
                token = lexer.GetNextToken();
            }
            
            Debug.Log($"Starting to construct primary from {token.GetSourceText()}");
            primaryOperand = NodeFactory.ConstructNode(new PrimaryNode(), lexer, token, out lastToken);
            children.Add(primaryOperand);
            Debug.Log($"Constructed primary in Unary, last token: {lastToken.GetSourceText()}");

            IsConst = primaryOperand.IsConst;
            
            return;
        }
        
        children.Add(NodeFactory.ConstructNode(new ReferenceNode(calledByForHeader), lexer, firstToken, out lastToken));
        
        Debug.Log($"Unary finished constructing ref, " +
                  $"last token: {lastToken.GetSourceText()}, constructed ref is {children.Last()}");
        
        if (lastToken is Is)
        {
            var token = lexer.GetNextToken();
            children.Add(NodeFactory.ConstructNode(new TypeIndicatorNode(), lexer, token, out lastToken));
            Debug.Log($"Type indicator returned last token {lastToken}");
        }
        
        Debug.Log($"Unary returning {lastToken.GetSourceText()} as last token");
    }

    private bool IsPrimaryOperator(Token token)
    {
        return token is Plus || token is Minus || token is Not;
    }
    
    protected override void Calculate()
    {
        // Only calculating in case of [ op ] const-primary
        var operandType = primaryOperand.GetValueType();
        if (operandType == ConstValueType.Int || operandType == ConstValueType.Real)
        {
            if (primaryOperator == null || primaryOperator is Plus || primaryOperator is Minus)
            {
                var multiplier = primaryOperator is Minus ? -1 : 1; 
                ValueType = primaryOperand.GetValueType();

                if (ValueType == ConstValueType.Real)
                {
                    RealValue = primaryOperand.GetRealValue() * multiplier;
                }
                else if (ValueType == ConstValueType.Int)
                {
                    IntValue = primaryOperand.GetIntValue() * multiplier;
                }
            }
            else // Not
            {
                ValueType = ConstValueType.Boolean;
                
                double operandValue = operandType == ConstValueType.Int
                    ? primaryOperand.GetIntValue()
                    : primaryOperand.GetRealValue();
                
                // i.e. not 1 == false
                BoolValue = operandValue == 0;
            }
        }
        else // Bool operand
        {
            if (primaryOperator is Plus || primaryOperator is Minus)
            {
                throw new SemanticException($"Cannot apply {primaryOperator.GetSourceText()} to bool");
            }
            
            ValueType = ConstValueType.Boolean;
            BoolValue = primaryOperator is Not
                ? !primaryOperand.GetBoolValue()
                : primaryOperand.GetBoolValue();
        }
    }
}
