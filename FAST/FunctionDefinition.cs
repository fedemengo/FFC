using System.Collections.Generic;
namespace FFC.FAST
{
    class FunctionDefinition : FPrimary
    {
        public ParameterList parameters;
        public FType returnType;
        public StatementList body;

        public FunctionDefinition(ParameterList parameters, FType returnType, StatementList body)
        {
            this.parameters = parameters;
            this.returnType = returnType;
            this.body = body;
        }
    }

    class ParameterList : FASTNode
    {
        public List<Parameter> parameters;
        public ParameterList(Parameter p)
        {
            parameters = new List<Parameter>{p};
        }
        public ParameterList()
        {
            parameters = new List<Parameter>();
        }
    }

    class Parameter : FASTNode
    {
        public Identifier id;
        public FType type;

        public Parameter(Identifier id, FType type)
        {
            this.id = id;
            this.type = type;
        }
    }
}