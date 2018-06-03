using System.Collections.Generic;
using System;
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
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Function definition");
            parameters.Print(tabs + 1);
            if(returnType != null) returnType.Print(tabs + 1);
            body.Print(tabs + 1);
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
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Parameter list");
            foreach(Parameter p in parameters)
                p.Print(tabs + 1);
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
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Parameter");
            id.Print(tabs + 1);
            type.Print(tabs + 1);
        }
    }
}