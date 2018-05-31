using System.Collections.Generic;
namespace FFC.FAST
{
    class FunctionDefinition : FPrimary
    {
        public List<Parameter> parameters;
        public FType returnType;
        public List<FStatement> body;
    }
    class Parameter : FASTNode
    {
        public Identifier id;
        public FType type;
    }
}