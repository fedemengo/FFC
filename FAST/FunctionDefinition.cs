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
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            FunctionType t = GetValueType(st) as FunctionType;
            TypeBuilder function = Generator.GetFunction(currentType, t);

            //we now need to emit proper method
            List<Type> paramTypes = new List<Type>();
            parameters.parameters.ForEach(word => paramTypes.Add(word.type.GetRunTimeType()));
            MethodBuilder funcMeth = function.DefineMethod("Invoke", MethodAttributes.Public, returnType.GetRunTimeType(), paramTypes.ToArray());
            
            //just generate code inside the method
            var funcGen = funcMeth.GetILGenerator();
            body.Generate(funcGen, function, st, exitLabel, conditionLabel);

            //Function class is now ready to be created
            function.CreateType();

            //create instance of function
            generator.Emit(OpCodes.Newobj, function.GetConstructors()[0]);

            //put function on the stack
            generator.Emit(OpCodes.Ldftn, funcMeth);

            //get delegate type
            var delegateType = Generator.FunctionTypes[valueType as FunctionType];

            //emits delegate
            generator.Emit(OpCodes.Newobj, delegateType.GetConstructors()[0]);
        }
        public override void BuildType(SymbolTable st)
        {
            var t = new FunctionType(null, null);
            t.returnType = body.GetValueType(st);
            t.paramTypes = new TypeList();
            foreach(var p in parameters.parameters)
                t.paramTypes.Add(p.GetValueType(st));
            valueType = t;
            if(returnType == null) returnType = t.returnType;
            else if(returnType.GetRunTimeType() != t.returnType.GetRunTimeType())
                throw new NotImplementedException($"{Span} - Returned type {t.returnType} doesn't match declared tpye {returnType}.");

            Generator.AddFunctionType(t);
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