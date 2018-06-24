using System.Collections.Generic;
using FFC.FGen;

namespace FFC.FAST
{
    public abstract class FPrimary : FSecondary
    {
        /* inherited by
            FValue
            Conditional
            FunctionDef
            ArrayDef
            MapDef
            TupleDef //which implies (Expression)            
        */
    }
}   