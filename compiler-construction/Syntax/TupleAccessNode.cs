using compiler_construction.Intrepretation;
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

    public WhatTupleReference GetWhatTupleReference()
    {
        return _whatTupleReference;
    }

    public override void ReadTokens(out Token lastToken)
    {
        var token = lexer.GetNextToken();
        if (token is Identifier)
        {
            children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, token));
            _whatTupleReference = WhatTupleReference.TupleByIdent;
        }
        else if (token is Int)
        {
            children.Add(NodeFactory.ConstructNode(new IntegerLiteral(), lexer, token));
            _whatTupleReference = WhatTupleReference.TupleByLiteral;
        }
        else
        {
            throw new UnexpectedTokenException($"For tuple access expected int literal or identifier, got {token}");
        }
        
        lastToken = lexer.GetNextToken();
    }
}
