using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using FFC.FParser;
using FFC.FGen;

namespace FFC.FAST
{
    public class FunctionDefinition : FPrimary
    {
        public ParameterList parameters;
        public FType returnType;
        public StatementList body;

        public FunctionDefinition(ParameterList parameters, FType returnType, StatementList body, TextSpan span)
        {
            this.Span = span;
            this.parameters = parameters;
            this.returnType = returnType;
            this.body = body;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Function definition");
            parameters.Print(tabs + 1);
            if(returnType != null) returnType.Print(tabs + 1);
            body.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            //only generates body
            body.Generate(generator, st);
        }
    }

    public class ParameterList : FASTNode
    {
        public List<Parameter> parameters;
        public ParameterList(Parameter p, TextSpan span)
        {
            this.Span = span;
            parameters = new List<Parameter>{p};
        }
        public ParameterList(TextSpan span = null)
        {
            this.Span = span;
            parameters = new List<Parameter>();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Parameter list");
            foreach(Parameter p in parameters)
                p.Print(tabs + 1);
        }
    }

    public class Parameter : FASTNode
    {
        public Identifier id;
        public FType type;

        public Parameter(Identifier id, FType type, TextSpan span)
        {
            this.Span = span;
            this.id = id;
            this.type = type;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Parameter");
            id.Print(tabs + 1);
            type.Print(tabs + 1);
        }
    }
}