using compiler_construction.Syntax;
using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class AssignmentInterpreter
{
    private AssignmentNode _assignment;

    public AssignmentInterpreter(AssignmentNode assignment)
    {
        _assignment = assignment;
    }

    public void Interpret()
    {
        ReferenceNode reference = (ReferenceNode)_assignment.GetChildren().First();
        ExpressionNode expression = (ExpressionNode)_assignment.GetChildren().Last();
        if (reference.getWhatReference() is WhatReference.Ident)
        {
            if (Interpreter.GetIdentifiers()[reference.GetIdentifier()] == null)
            {
                throw new InterpretationException($"Interpreter: no identifier {reference.GetIdentifier().GetValue()} is found");
            }
            Interpreter.GetIdentifiers()[reference.GetIdentifier()] = expression;
        }

        if (reference.getWhatReference() is WhatReference.Tuple)
        {
            IdentifierNode tupleName = (IdentifierNode)reference.GetChildren().First();
            if (Interpreter.GetIdentifiers()[tupleName] == null)
            {
                throw new InterpretationException($"Interpreter: no tuple called {tupleName.GetValue()} is found");
            }
            TupleAccessNode tupleElement = (TupleAccessNode)reference.GetChildren().Skip(1).First();
            if (tupleElement.GetWhatTupleReference() is WhatTupleReference.TupleByIdent)
            {
                IdentifierNode tupleElementName = (IdentifierNode)tupleElement.GetChildren().First();
            }
        }
        
        // DO NOT INTERPRET REFERENCE IF YOU ARE HERE
        
    }
}