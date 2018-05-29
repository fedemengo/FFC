using System;
using System.Collections.Generic;

namespace AbstractSyntaxTree
{
    class Starting
    {
        public DecList list;
    }
    class DecList
    {
        public List<Declaration> list;
    }
    class Declaration
    {
        public Identifier id;
        public FType t;
        public Expression ex;
        Declaration(Identifier id, FType t, Expression ex)
        {
            
        }
    }
    class Parameter
    {
        public Identifier ID;
        public FType type;
    }

}