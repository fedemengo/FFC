using System.Collections.Generic;
namespace FFC.FAST
{
    class FunctionDefinition : FPrimary
    {
        public List<Parameter> parameters;
        public FType returnType;
        public List<FStatement> body;

        public FunctionDefinition(List<Parameter> parameters, FType returnType, List<FStatement> body)
        {
            this.parameters = parameters;
            this.returnType = returnType;
            this.body = body;
        }
    }
    class Parameter : FASTNode
    {
        public Identifier id;
        public FType type;

        public Parameter(Indexer id, FType type)
        {
            this.id = id;
            this.type = type;
        }
    }
}