using System.Collections.Generic;

namespace FAST
{
    abstract class FPrimary : FSecondary
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