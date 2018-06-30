using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

using FFC.FAST;
using FFC.FGen;

namespace FFC.FGen
{
    public partial class Generator
    {
        //call reset to have it ready to compile a new file
        public static void Reset() => throw new NotImplementedException("Reset of the generator status is not currently implemented");
        
        //stored globally to emit delegate types for function types
        public static TypeBuilder programType;
        //The function we need to call if found
        public static string StartFunction {get ; private set;}
        private static ILGenerator mainGen;

        public static void EmitStartFunction(object toCall, FunctionType type)
        {
            if(type.ParamsList.Types.Count > 0)
                throw new NotImplementedException($"{type.Span} - Cannot start program with a function that takes parameters");
            //load on stack
            EmitLoad(mainGen, toCall);
            //call invoke on the delegate type
            mainGen.Emit(OpCodes.Callvirt, FunctionTypes[type].GetMethod("Invoke"));
            //Pop call if needed
            if(type.ReturnType is VoidType == false) mainGen.Emit(OpCodes.Pop);
            mainGen.Emit(OpCodes.Ret);
        }

        //might we need to store more stuff globally ?

        public static bool Generate(string filePath, DeclarationStatementList stms, string start)
        {
            string name = filePath.Split('/')[1].Split('.')[0];
            //name of function to invoke in Program.Main
            StartFunction = start;
            
            AppDomain appDomain = System.Threading.Thread.GetDomain();
            AssemblyName asmName = new AssemblyName(name);

            AssemblyBuilder asmBuilder = appDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule(asmName.Name, name + ".exe", true);

            moduleBuilder.CreateGlobalFunctions();

            /* Class 'Program' that will run all the statements */
            
            programType = moduleBuilder.DefineType("Program", TypeAttributes.Public);
            ConstructorBuilder progConstr = programType.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                                                        CallingConventions.Standard,
                                                        new Type[0]);
                            
            ILGenerator progConstrGen = progConstr.GetILGenerator();
            progConstrGen.Emit(OpCodes.Ldarg_0);
            progConstrGen.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
            progConstrGen.Emit(OpCodes.Ret);
            
            /* Program.Main method */
            MethodBuilder mainMeth = programType.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), new Type[] { typeof(string[]) });
            //Assign it to static field so that we can emit first call
            mainGen = mainMeth.GetILGenerator();
            
            //generates all the statements in main
            stms.Generate(mainGen, programType, new SymbolTable());

            //end of main
            
            // set assembly entry point
            asmBuilder.SetEntryPoint(mainMeth);
            // create program type
            programType.CreateType();

            asmBuilder.Save(name + ".exe");

            return true;
        }

        public static void EmitLoad(ILGenerator generator, object builder)
        {
            if(builder is LocalBuilder)
                generator.Emit(OpCodes.Ldloc, builder as LocalBuilder);
            else if (builder is ParameterBuilder)
                generator.Emit(OpCodes.Ldarg, (builder as ParameterBuilder).Position);
            else if (builder is FieldBuilder)
            {
                var code = (builder as FieldBuilder).IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld;
                generator.Emit(code, builder as FieldBuilder);
            }
            else throw new NotImplementedException($"Cannot load {builder.GetType()}");
        }
        public static void EmitStore(ILGenerator generator, object builder)
        {
            if(builder is LocalBuilder)
                generator.Emit(OpCodes.Stloc, builder as LocalBuilder);
            else if (builder is ParameterBuilder)
                generator.Emit(OpCodes.Starg, (builder as ParameterBuilder).Position);
            else if (builder is FieldBuilder)
            {
                var code = (builder as FieldBuilder).IsStatic ? OpCodes.Stsfld : OpCodes.Stfld;
                generator.Emit(code, builder as FieldBuilder);
            }
            else throw new NotImplementedException($"Cannot load {builder.GetType()}");
        }

        public static Dictionary<FunctionType, TypeBuilder> FunctionTypes = new Dictionary<FunctionType, TypeBuilder>();
        
        //we shall probably split this in many files and abuse of mr partial class
        public static void AddFunctionType(FunctionType f)
        {
            string name = GetNextDelName();
            TypeBuilder tdelegate = programType.DefineNestedType(name, TypeAttributes.AutoClass | 
                                                                          TypeAttributes.AnsiClass |
                                                                          TypeAttributes.Sealed |
                                                                          TypeAttributes.NestedPublic, typeof(System.MulticastDelegate));

            ConstructorBuilder delegateConstr = tdelegate.DefineConstructor(MethodAttributes.Public | 
                                                                        MethodAttributes.HideBySig | 
                                                                        MethodAttributes.SpecialName | 
                                                                        MethodAttributes.RTSpecialName,
                                                                        CallingConventions.Standard, Type.EmptyTypes);

            delegateConstr.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            Type retType = f.ReturnType.GetRunTimeType();
            Type[] paramTypesArray = new Type[f.ParamsList.Types.Count];
            for(int i = 0; i < f.ParamsList.Types.Count; i++)
            {
                paramTypesArray[i] = f.ParamsList.Types[i].GetRunTimeType();
            }
            
            MethodBuilder methodInvoke = tdelegate.DefineMethod("Invoke", MethodAttributes.Public |
                                                                      MethodAttributes.HideBySig |
                                                                      MethodAttributes.NewSlot |
                                                                      MethodAttributes.Virtual, CallingConventions.Standard, 
                                                                      retType, paramTypesArray);
            methodInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            Type[] paramTypesArrayBeginInvoke = new Type[paramTypesArray.Length + 2];
            for(int i = 0; i < paramTypesArray.Length; i++)
                paramTypesArrayBeginInvoke[i] = paramTypesArray[i];
            paramTypesArrayBeginInvoke[paramTypesArray.Length] = typeof(AsyncCallback);
            paramTypesArrayBeginInvoke[paramTypesArray.Length + 1] = typeof(object);


            MethodBuilder methodBeginInvoke = tdelegate.DefineMethod("BeginInvoke", MethodAttributes.Public |
                                                                                    MethodAttributes.HideBySig |
                                                                                    MethodAttributes.NewSlot |
                                                                                    MethodAttributes.Virtual,
                                                                                    typeof(IAsyncResult), 
                                                                                    paramTypesArrayBeginInvoke);

            methodBeginInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            MethodBuilder methodEndInvoke = tdelegate.DefineMethod("EndInvoke", MethodAttributes.Public | 
                                                                                MethodAttributes.HideBySig | 
                                                                                MethodAttributes.NewSlot | 
                                                                                MethodAttributes.Virtual, 
                                                                                null, new Type[] { typeof(IAsyncResult)});
            methodEndInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            //Creates the delegate type
            tdelegate.CreateType();

            //assign the newly created type to the dictionary
            FunctionTypes[f] = tdelegate;
        }
        private static int funcCount = 0, delCount = 0;
        private static string GetNextFuncName() => "___f" + (funcCount++).ToString();
        private static string GetNextDelName() => "___d" + (delCount++).ToString();

        public static TypeBuilder GetDelegate(FunctionType funcType)
        {
            if(FunctionTypes.ContainsKey(funcType) == false)
                AddFunctionType(funcType);
            return FunctionTypes[funcType];
        }
        public static TypeBuilder GetFunction(TypeBuilder parentType, FunctionType funcType)
        {
            if(FunctionTypes.ContainsKey(funcType) == false)
                AddFunctionType(funcType);

            //Emit nested function type
            TypeBuilder funcClass = parentType.DefineNestedType(GetNextFuncName(), TypeAttributes.AutoClass | 
                                                                            TypeAttributes.AnsiClass |
                                                                            TypeAttributes.Sealed |
                                                                            TypeAttributes.NestedPublic, typeof(object));

            //Define a field with pointer to parent - this will probably stop the GC to do bad stuff
            FieldBuilder funcFField = funcClass.DefineField("outerClass", funcClass.DeclaringType, FieldAttributes.Public);
            
            //Define (empty) constructor

            ConstructorBuilder funcClassCtor = funcClass.DefineConstructor(MethodAttributes.Public | 
                                                                        MethodAttributes.HideBySig | 
                                                                        MethodAttributes.SpecialName | 
                                                                        MethodAttributes.RTSpecialName,
                                                                        CallingConventions.Standard, Type.EmptyTypes);
            //Emit constructor code
            ILGenerator funcClassCtorGen = funcClassCtor.GetILGenerator();
            funcClassCtorGen.Emit(OpCodes.Ldarg_0);
            funcClassCtorGen.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
            funcClassCtorGen.Emit(OpCodes.Ret);
            
            //Type is now ready to emit everything
            return funcClass;

        }
    }
}