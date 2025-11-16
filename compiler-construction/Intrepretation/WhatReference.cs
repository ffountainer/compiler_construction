namespace compiler_construction.Intrepretation;

public enum WhatReference
{
    Ident,
    Array,
    Call,
    Tuple,
}

public enum WhatTupleReference
{
    TupleByIdent,
    TupleByLiteral
}