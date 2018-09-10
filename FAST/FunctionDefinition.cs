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

            /*
            before generating the body, we need to capture all local variables:
                - before body generation, create a list of "names" that will need to
                  be created as Fields in the function type
                - this names will need to be updated on the symbol table!
                - after body generation, store locals in newly created fields
                
            we dont':
                - use old symbol table to track "untracked variables", as this would
                    (or i think it would) require to modifiy st structure
                - delete useless symbols from newSt, as they shouldnt take so much memory
                    and would probably take even more if removed because of persistent implementation
            */

            SymbolTable newSt = st;
            SortedSet<string> paramNames = new SortedSet<string>();
            ParamsList.Params.ForEach(x => paramNames.Add(x.Id.Name));
            SortedSet<string> toCapture = Generator.GetUndeclared(Body, paramNames);
            //Names to be skipped
            SortedSet<string> toSkip = new SortedSet<string>();
            //Add field builders to symboltable
            foreach(string s in toCapture)
            {
                //If name isn't in SymTable
                if(st.Contains(s) == false)
                    throw new FCompilationException($"{Span} - Couldn't resolve name {s}");
                var z = st.Find(s);
                //we skip static fields are they are global variables which can be captured anyway
                if(z.Builder is FieldBuilder)
                    if((z.Builder as FieldBuilder).IsStatic)
                    {
                        toSkip.Add(s);
                        continue;
                    }
                //create a field and assign to new symbol table
                FieldBuilder f = function.DefineField(s, z.Type.GetRunTimeType(), FieldAttributes.Public);
                newSt = newSt.Assign(s, new NameInfo(f, z.Type));
            }

            //remove globals
            foreach(string s in toSkip)
                toCapture.Remove(s);

            //just generate code inside the method
            var funcGen = funcMeth.GetILGenerator();
            //we use the updated symbol table this time
            Body.Generate(funcGen, function, newSt, exitLabel, conditionLabel);

            //Function class is now ready to be created
            var fType = function.CreateType();

            //create instance of function
            generator.Emit(OpCodes.Newobj, function.GetConstructors()[0]);

            //put function on tmp variable, store and load fields

            var tmpF = generator.DeclareLocal(fType);
            Generator.EmitStore(generator, tmpF);


            //load fields
            foreach(string s in toCapture)
            {
                Generator.EmitLoad(generator, tmpF);
                Generator.EmitLoad(generator, st.Find(s).Builder);
                Generator.EmitStore(generator, newSt.Find(s).Builder);
            }

            //put function back on the stack to create actual delegate
            Generator.EmitLoad(generator, tmpF);
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
            //I think it's ok to tag it as void and let other built in exception find possible problems
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