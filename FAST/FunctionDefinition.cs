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
        public ParameterList ParamsList {get; set;}
        public FType ReturnType {get; set;}
        public StatementList Body {get; set;}

        public FunctionDefinition(ParameterList parameters, FType returnType, StatementList body, TextSpan span)
        {
            ParamsList = parameters;
            ReturnType = returnType;
            Body = body;
            Span = span;
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Function definition");
            ParamsList.Print(tabs + 1);
            if(ReturnType != null) ReturnType.Print(tabs + 1);
            Body.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            FunctionType t = GetValueType(st) as FunctionType;
            if(t == null) throw new FCompilationException($"{Span} - Couldn't determine function type");

            TypeBuilder function = Generator.GetFunction(currentType, t);

            //we now need to emit proper method
            List<Type> paramTypes = new List<Type>();
            ParamsList.Params.ForEach(word => paramTypes.Add(word.Type.GetRunTimeType()));
            MethodBuilder funcMeth = function.DefineMethod("Invoke", MethodAttributes.Public, ReturnType.GetRunTimeType(), paramTypes.ToArray());
            
            // define parameter type
            for(int i=0; i<ParamsList.Params.Count; ++i)
            {
                // add +1 to account for "this" as parameter
                ParameterBuilder paramBuilder = funcMeth.DefineParameter(i + 1, ParameterAttributes.In, ParamsList.Params[i].Id.Name);
                st = st.Assign(ParamsList.Params[i].Id.Name, new NameInfo(paramBuilder, ParamsList.Params[i].Type));
            }

            //just generate code inside the method
            var funcGen = funcMeth.GetILGenerator();
            Body.Generate(funcGen, function, st, exitLabel, conditionLabel);

            //Function class is now ready to be created
            function.CreateType();

            //create instance of function
            generator.Emit(OpCodes.Newobj, function.GetConstructors()[0]);

            //put function on the stack
            generator.Emit(OpCodes.Ldftn, funcMeth);

            //get delegate type
            var delegateType = Generator.FunctionTypes[ValueType as FunctionType];

            //emits delegate
            generator.Emit(OpCodes.Newobj, delegateType.GetConstructors()[0]);
        }
        public override void BuildValueType(SymbolTable st)
        {
            //we prepare function type
            var t = new FunctionType(null, null);
            //get type of each param
            t.ParamsList = new TypeList();
            foreach(var p in ParamsList.Params)
            {
                //add params to symbol table
                st = st.Assign(p.Id.Name, new NameInfo(null, p.Type));
                //get type of each parameter
                t.ParamsList.Add(p.Type);
            }
            //return type is body type
            t.ReturnType = Body.GetValueType(st);

            //if there is not a type caught, we either can't resolve it or it's a void function
            //i think it's ok to tag it as void and let other built in exception find possible problems
            //TODO: check all cases
            //also, we append a return at the end of the function
            if(t.ReturnType == null)
            {
                // throw new FCompilationException($"{Span} - Couldn't determine function return type");
                t.ReturnType = new DeducedVoidType();
                Body.StmsList.Add(new ReturnStatement(new TextSpan(Span)));
            }

            //if ret type was not specified
            if(ReturnType == null) ReturnType = t.ReturnType;

            //if wrong type was specified
            if(FType.SameType(ReturnType, t.ReturnType) == false)
                throw new FCompilationException($"{Span} - Returned type {t.ReturnType} doesn't match declared tpye {ReturnType}.");
            
            //assign function value type
            ValueType = t;

            Generator.AddFunctionType(t);
        }
    }

    public class ParameterList : FASTNode
    {
        public List<Parameter> Params {get; set;}
        public ParameterList(Parameter p, TextSpan span)
        {
            Params = new List<Parameter>{p};
            Span = span;
        }
        public ParameterList(TextSpan span = null)
        {
            Params = new List<Parameter>();
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Parameter list");
            foreach(Parameter p in Params)
                p.Print(tabs + 1);
        }
    }

    public class Parameter : FASTNode
    {
        public Identifier Id {get; set;}
        public FType Type {get; set;}

        public Parameter(Identifier id, FType type, TextSpan span)
        {
            Id = id;
            Type = type;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Parameter");
            Id.Print(tabs + 1);
            Type.Print(tabs + 1);
        }

        public override void BuildValueType(SymbolTable st) => ValueType = Type;
    }
}