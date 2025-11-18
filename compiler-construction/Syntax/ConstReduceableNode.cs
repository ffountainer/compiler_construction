using compiler_construction.Interpretation;
using compiler_construction.Semantics;
using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

/// <summary>
/// Represents a node that can be reduced to constant value
/// </summary>
public abstract class ConstReduceableNode : TreeNode
{
    protected ConstValueType ValueType;
    protected bool BoolValue;
    protected double RealValue;
    protected int IntValue;
    protected string StringValue;
    protected List<TupleElementNode> TupleValue = new List<TupleElementNode>();
    protected List<ExpressionNode> ArrayValue =  new List<ExpressionNode>();
    protected List<String> Arguments =  new List<String>();
    protected WhatFunction WhatFunc;
    protected List<StatementNode> Body = new List<StatementNode>();
    protected ExpressionNode shortFuncExpr;

    public bool IsConst;

    public ConstValueType GetValueType()
    {
        return ValueType;
    }

    public bool GetBoolValue()
    {
        return BoolValue;
    }

    public double GetRealValue()
    {
        return RealValue;
    }

    public int GetIntValue()
    {
        return IntValue;
    }
    
    protected virtual bool CanReduce()
    {
        return IsConst;
    }
    
    public void Reduce()
    {
        if (!CanReduce())
        {
            Debug.Log($"Node {ToString()} cannot be reduced");
            return;
        }
        
        Calculate();
        Debug.Log($"For node {GetName()} the tree before optimisations is");
        if (Debug.debug)
        {
            foreach (TreeNode child in children)
            {
                child.PrintTree();
            }
        }
        children = new List<TreeNode>();
        children.Add(ConstructTrunk());
        
        Debug.Log($"For node {GetName()} reduced tree is");
        if (Debug.debug)
        {
            foreach (TreeNode child in children)
            {
                child.PrintTree();
            }
        }
    }

    /// <summary>
    /// Children should implement this to set one of the values and the value type,
    /// after calculating things based on operands and operators
    /// </summary>
    protected abstract void Calculate();

    private TreeNode GetTypedLiteral()
    {
        switch (ValueType)
        {
            case ConstValueType.Boolean:
                return new BooleanLiteral().WithValue(BoolValue);
            case ConstValueType.Real:
                return new RealLiteral().WithValue(RealValue);
            case ConstValueType.Int:
            default:
                return new IntegerLiteral().WithValue(IntValue);
        }
    }

    private TreeNode ConstructTrunk()
    {
        Debug.Log($">> Will construct trunk for node {GetName()} of type {GetType()}");
        
        // Expression > Relation > Factor > Term > Unary > Primary > Literal > TypedLiteral
        
        var node = GetTypedLiteral();

        if (GetType() == typeof(LiteralNode)) return node;
        node = new LiteralNode().AddChild(node);

        if (GetType() == typeof(PrimaryNode)) return node;
        node = new PrimaryNode().AddChild(node);
        
        if (GetType() == typeof(UnaryNode))  return node;
        node = new UnaryNode().AddChild(node);
        
        if (GetType() == typeof(TermNode)) return node;
        node = new TermNode().AddChild(node);
        
        if (GetType() ==  typeof(FactorNode)) return node;
        node = new FactorNode().AddChild(node);
        
        if (GetType() == typeof(RelationNode)) return node;
        node = new RelationNode().AddChild(node);
        
        if (GetType() == typeof(ExpressionNode)) return node;
        return new ExpressionNode().AddChild(node);
    }

    public double GetNumericalValue()
    {
        if (ValueType == ConstValueType.Boolean)
        {
            throw new SemanticException("Boolean reduceable does not have numeric value");
        }
        
        return ValueType ==  ConstValueType.Real ? RealValue : IntValue;
    }
}
