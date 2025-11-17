namespace compiler_construction.Interpretation;

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
    TupleByIndex
}