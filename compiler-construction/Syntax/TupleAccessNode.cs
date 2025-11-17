using compiler_construction.Interpretation;
using compiler_construction.Syntax.Literals;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Types;

namespace compiler_construction.Syntax;

public class TupleAccessNode : TreeNode
{
    private WhatTupleReference _whatTupleReference;
    public override string GetName()
    {
        return "TupleAccess";
    }

    private int tupleIndex;
    
    private IdentifierNode tupleAccessIdentifier;

    public IdentifierNode GetTupleAccessIdentifier()
    {
        return tupleAccessIdentifier;
    }

    public int GetTupleIndex()
    {
        return tupleIndex;
    }

    public WhatTupleReference GetWhatTupleReference()
    {
        return _whatTupleReference;
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();
        if (token is Identifier)
        {
            var node = NodeFactory.ConstructNode(new IdentifierNode(), lexer, token);
            children.Add(node);
            _whatTupleReference = WhatTupleReference.TupleByIdent;
            tupleAccessIdentifier = node;
        }
        else if (token is Int)
        {
            var indexInt = NodeFactory.ConstructNode(new IntegerLiteral(), lexer, token);
            children.Add(indexInt);
            _whatTupleReference = WhatTupleReference.TupleByIndex;
            tupleIndex = indexInt.Value;

        }
        else
        {
            throw new UnexpectedTokenException($"For tuple access expected int literal or identifier, got {token}");
        }
        
        lastToken = lexer.GetNextToken();
    }
}
