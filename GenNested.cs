using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

class Program
{
    public static void Main(string[] args)
    {
        string name = "test";
        AppDomain appDomain = Thread.GetDomain();
        AssemblyName asmName = new AssemblyName(name);

        AssemblyBuilder asmBuilder = appDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
        ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule(asmName.Name, name + ".exe", true);

        moduleBuilder.CreateGlobalFunctions();

        /* Class 'Program' that will run all the statements */
        TypeBuilder programType = moduleBuilder.DefineType("Program", TypeAttributes.Public);
        ConstructorBuilder progConstr = programType.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                                                    CallingConventions.Standard,
                                                    new Type[0]);
                        
        ILGenerator progConstrGen = progConstr.GetILGenerator();
        progConstrGen.Emit(OpCodes.Ldarg_0);
        progConstrGen.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
        progConstrGen.Emit(OpCodes.Ret);
        
#region lambda
    /* 
        TypeBuilder lambda1 = programType.DefineNestedType("Lambda1", TypeAttributes.NestedPublic);
        ConstructorBuilder lambdaConstr = lambda1.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                                                    CallingConventions.Standard,
                                                    new Type[0]);
        
        ILGenerator lambdaConstrGen = lambdaConstr.GetILGenerator();
        lambdaConstrGen.Emit(OpCodes.Ldarg_0);
        lambdaConstrGen.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
        lambdaConstrGen.Emit(OpCodes.Ret);

        MethodBuilder invoke = lambda1.DefineMethod("Invoke", MethodAttributes.Public, typeof(void), Type.EmptyTypes);
        ILGenerator invokeGen = invoke.GetILGenerator();
        invokeGen.Emit(OpCodes.Ldstr, "Maniezzo");
        invokeGen.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new  Type[]{typeof(string)}));
        invokeGen.Emit(OpCodes.Ret);

        MethodBuilder mSum = lambda1.DefineMethod("Sum", MethodAttributes.Public | 
                                                         MethodAttributes.HideBySig |
                                                         MethodAttributes.Virtual |
                                                         MethodAttributes.Final, CallingConventions.Standard,
                                                         typeof(System.Int32),
                                                         new Type[] { typeof(System.Int32), typeof(System.Int32) });
        mSum.SetImplementationFlags(MethodImplAttributes.Managed);
        ILGenerator sumIL = mSum.GetILGenerator();
        sumIL.Emit(OpCodes.Nop);
        sumIL.Emit(OpCodes.Ldarg_1);
        sumIL.Emit(OpCodes.Ldarg_2);
        sumIL.Emit(OpCodes.Sum);
        sumIL.Emit(OpCodes.Stloc_1);
        sumIL.Emit(OpCodes.Ldloc_1);
        sumIL.Emit(OpCodes.Ret);

        Type lambdaType = lambda1.CreateType(); 
        
        //mainMethGen.Emit(OpCodes.Newobj, lambda1.GetConstructor(Type.EmptyTypes));
        //mainMethGen.Emit(OpCodes.Call, lambda1.GetMethod("Invoke", Type.EmptyTypes));    
    */
#endregion lambda
        
        TypeBuilder tdelegate = programType.DefineNestedType("intIntInt", TypeAttributes.AutoClass | 
                                                                          TypeAttributes.AnsiClass |
                                                                          TypeAttributes.Sealed |
                                                                          TypeAttributes.NestedPublic, typeof(System.MulticastDelegate));

        ConstructorBuilder delegateConstr = tdelegate.DefineConstructor(MethodAttributes.Public | 
                                                                        MethodAttributes.HideBySig | 
                                                                        MethodAttributes.SpecialName | 
                                                                        MethodAttributes.RTSpecialName,
                                                                        CallingConventions.Standard, Type.EmptyTypes);
        delegateConstr.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);


        MethodBuilder methodInvoke = tdelegate.DefineMethod("Invoke", MethodAttributes.Public |
                                                                      MethodAttributes.HideBySig |
                                                                      MethodAttributes.NewSlot |
                                                                      MethodAttributes.Virtual, CallingConventions.Standard, 
                                                                      typeof(int), new Type[] {typeof(int), typeof(int)});
        methodInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

        MethodBuilder methodBeginInvoke = tdelegate.DefineMethod("BeginInvoke", MethodAttributes.Public |
                                                                                MethodAttributes.HideBySig |
                                                                                MethodAttributes.NewSlot |
                                                                                MethodAttributes.Virtual,
                                                                                typeof(IAsyncResult), 
                                                                                new Type[] {typeof(int), typeof(int), typeof(AsyncCallback), typeof(object) });
        methodBeginInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

        MethodBuilder methodEndInvoke = tdelegate.DefineMethod("EndInvoke", MethodAttributes.Public | 
                                                                            MethodAttributes.HideBySig | 
                                                                            MethodAttributes.NewSlot | 
                                                                            MethodAttributes.Virtual, 
                                                                            null, new Type[] { typeof(IAsyncResult)});
        methodEndInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
        
        Type delegateType = tdelegate.CreateType();

        TypeBuilder delIntInt = programType.DefineNestedType("delIntInt", TypeAttributes.AutoClass | 
                                                                          TypeAttributes.AnsiClass |
                                                                          TypeAttributes.Sealed |
                                                                          TypeAttributes.NestedPublic, typeof(System.MulticastDelegate));

        ConstructorBuilder delIntIntConstr = delIntInt.DefineConstructor(MethodAttributes.Public | 
                                                                        MethodAttributes.HideBySig | 
                                                                        MethodAttributes.SpecialName | 
                                                                        MethodAttributes.RTSpecialName,
                                                                        CallingConventions.Standard, Type.EmptyTypes);
        delIntIntConstr.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);


        MethodBuilder delIntIntmethodInvoke = delIntInt.DefineMethod("Invoke", MethodAttributes.Public |
                                                                      MethodAttributes.HideBySig |
                                                                      MethodAttributes.NewSlot |
                                                                      MethodAttributes.Virtual, CallingConventions.Standard, 
                                                                      delegateType, new Type[] {typeof(int), typeof(int)});
        delIntIntmethodInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

        MethodBuilder delIntIntmethodBeginInvoke = delIntInt.DefineMethod("BeginInvoke", MethodAttributes.Public |
                                                                                MethodAttributes.HideBySig |
                                                                                MethodAttributes.NewSlot |
                                                                                MethodAttributes.Virtual,
                                                                                typeof(IAsyncResult), 
                                                                                new Type[] {typeof(int), typeof(int), typeof(AsyncCallback), typeof(object) });
        delIntIntmethodBeginInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

        MethodBuilder delIntIntEndInvoke = delIntInt.DefineMethod("EndInvoke", MethodAttributes.Public | 
                                                                            MethodAttributes.HideBySig | 
                                                                            MethodAttributes.NewSlot | 
                                                                            MethodAttributes.Virtual, 
                                                                            null, new Type[] { typeof(IAsyncResult)});
        delIntIntEndInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
        
        Type delIntIntType = delIntInt.CreateType();

#region FuncClass
        TypeBuilder sumFunction = programType.DefineNestedType("sumFunc", TypeAttributes.AutoClass | 
                                                                          TypeAttributes.AnsiClass |
                                                                          TypeAttributes.Sealed |
                                                                          TypeAttributes.NestedPublic, typeof(object));

        ConstructorBuilder sumFuncCtor = sumFunction.DefineConstructor(MethodAttributes.Public | 
                                                                        MethodAttributes.HideBySig | 
                                                                        MethodAttributes.SpecialName | 
                                                                        MethodAttributes.RTSpecialName,
                                                                        CallingConventions.Standard, Type.EmptyTypes);
        ILGenerator sumFuncCtorGen = sumFuncCtor.GetILGenerator();
        sumFuncCtorGen.Emit(OpCodes.Ldarg_0);
        sumFuncCtorGen.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
        sumFuncCtorGen.Emit(OpCodes.Ret);

        FieldBuilder sumFuncField = sumFunction.DefineField("x", typeof(int), FieldAttributes.Public);

#region InnerFuncClass
        // nested class
            TypeBuilder innerFunc = sumFunction.DefineNestedType("sumFuncInner", TypeAttributes.AutoClass | 
                                                                            TypeAttributes.AnsiClass |
                                                                            TypeAttributes.Sealed |
                                                                            TypeAttributes.NestedPublic, typeof(object));

            FieldBuilder innerFField = innerFunc.DefineField("outerClass", innerFunc.DeclaringType, FieldAttributes.Public);
            FieldBuilder innerSField = innerFunc.DefineField("aField", typeof(int), FieldAttributes.Public);

            ConstructorBuilder innerFuncCtor = innerFunc.DefineConstructor(MethodAttributes.Public | 
                                                                        MethodAttributes.HideBySig | 
                                                                        MethodAttributes.SpecialName | 
                                                                        MethodAttributes.RTSpecialName,
                                                                        CallingConventions.Standard, Type.EmptyTypes);

            ILGenerator innerFuncCtorGen = innerFuncCtor.GetILGenerator();
            innerFuncCtorGen.Emit(OpCodes.Ldarg_0);
            innerFuncCtorGen.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
            innerFuncCtorGen.Emit(OpCodes.Ret);

            MethodBuilder innerMethod = innerFunc.DefineMethod("aMethod", MethodAttributes.Public, typeof(int), new Type[]{typeof(int), typeof(int)});
            ILGenerator innerMethodGen = innerMethod.GetILGenerator();
            innerMethodGen.Emit(OpCodes.Ldarg_1);
            innerMethodGen.Emit(OpCodes.Ldarg_0);
            innerMethodGen.Emit(OpCodes.Ldfld, innerSField);
            innerMethodGen.Emit(OpCodes.Add);
            innerMethodGen.Emit(OpCodes.Ldarg_2);
            innerMethodGen.Emit(OpCodes.Add);
            innerMethodGen.Emit(OpCodes.Ldarg_0);
            innerMethodGen.Emit(OpCodes.Ldfld, innerFField);
            innerMethodGen.Emit(OpCodes.Ldfld, sumFuncField);
            innerMethodGen.Emit(OpCodes.Add);
            innerMethodGen.Emit(OpCodes.Ret);

            Type innerFuncType = innerFunc.CreateType();

#endregion        

        MethodBuilder sumFuncGenDelegate = sumFunction.DefineMethod("F1", MethodAttributes.Public, delegateType, new Type[]{typeof(int), typeof(int)});
        ILGenerator sumFuncGenDelegateGen = sumFuncGenDelegate.GetILGenerator();
        LocalBuilder innerClass = sumFuncGenDelegateGen.DeclareLocal(innerFuncType);
        sumFuncGenDelegateGen.Emit(OpCodes.Newobj, innerFuncType.GetConstructor(Type.EmptyTypes));        
        sumFuncGenDelegateGen.Emit(OpCodes.Stloc, innerClass);

        sumFuncGenDelegateGen.Emit(OpCodes.Ldloc, innerClass);        
        sumFuncGenDelegateGen.Emit(OpCodes.Ldarg_0);
        sumFuncGenDelegateGen.Emit(OpCodes.Stfld, innerFField);        
        
        sumFuncGenDelegateGen.Emit(OpCodes.Ldloc, innerClass);        
        sumFuncGenDelegateGen.Emit(OpCodes.Ldarg_1);
        sumFuncGenDelegateGen.Emit(OpCodes.Stfld, innerSField);
        
        sumFuncGenDelegateGen.Emit(OpCodes.Ldloc, innerClass);        
        sumFuncGenDelegateGen.Emit(OpCodes.Ldftn, innerMethod);
        sumFuncGenDelegateGen.Emit(OpCodes.Newobj, delegateType.GetConstructor(Type.EmptyTypes));
        sumFuncGenDelegateGen.Emit(OpCodes.Ret);

        Type sumFuncType = sumFunction.CreateType();

#endregion

        MethodBuilder sumMeth = programType.DefineMethod("Sum", MethodAttributes.Public | MethodAttributes.Static, typeof(int), new Type[] { typeof(int), typeof(int) });
        ILGenerator sumMethIL = sumMeth.GetILGenerator();
        sumMethIL.Emit(OpCodes.Ldarg_0);
        sumMethIL.Emit(OpCodes.Ldarg_1);
        sumMethIL.Emit(OpCodes.Add);
        sumMethIL.Emit(OpCodes.Ret);
    
        MethodBuilder subMeth = programType.DefineMethod("Sub", MethodAttributes.Public | MethodAttributes.Static, typeof(int), new Type[] { typeof(int), typeof(int) });
        ILGenerator subMethIL = subMeth.GetILGenerator();
        subMethIL.Emit(OpCodes.Ldarg_0);
        subMethIL.Emit(OpCodes.Ldarg_1);
        subMethIL.Emit(OpCodes.Sub);
        subMethIL.Emit(OpCodes.Ret);

        FieldBuilder fFirst = programType.DefineField("firstField", delegateType, FieldAttributes.Public | FieldAttributes.Static);

        /* Methods */
    // main method
        MethodBuilder mainMeth = programType.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), new Type[] { typeof(string[]) });
        ILGenerator mainMethGen = mainMeth.GetILGenerator();

/*
#region oldTest
        Label fFiledzero = mainMethGen.DefineLabel();
        LocalBuilder localDelegate1 = mainMethGen.DeclareLocal(delegateType);
        LocalBuilder localDelegate2 = mainMethGen.DeclareLocal(delegateType);
        mainMethGen.Emit(OpCodes.Ldnull);   // ???????????????????????????
        mainMethGen.Emit(OpCodes.Ldftn, sumMeth);
        mainMethGen.Emit(OpCodes.Newobj, delegateType.GetConstructor(Type.EmptyTypes));
        mainMethGen.Emit(OpCodes.Stsfld, fFirst);
        mainMethGen.Emit(OpCodes.Ldsfld, fFirst);
        mainMethGen.Emit(OpCodes.Stloc, localDelegate1);

        mainMethGen.Emit(OpCodes.Ldnull);   // ???????????????????????????
        mainMethGen.Emit(OpCodes.Ldftn, subMeth);
        mainMethGen.Emit(OpCodes.Newobj, delegateType.GetConstructor(Type.EmptyTypes));
        mainMethGen.Emit(OpCodes.Stsfld, fSecond);
        mainMethGen.Emit(OpCodes.Ldsfld, fSecond);
        mainMethGen.Emit(OpCodes.Stloc, localDelegate2);

        mainMethGen.Emit(OpCodes.Ldloc, localDelegate1);
        mainMethGen.Emit(OpCodes.Ldc_I4, 3);
        mainMethGen.Emit(OpCodes.Ldc_I4, 5);
        mainMethGen.Emit(OpCodes.Callvirt, delegateType.GetMethod("Invoke", new Type[]{typeof(int), typeof(int)}));
        mainMethGen.Emit(OpCodes.Call,typeof(System.Console).GetMethod("WriteLine", new Type[]{typeof(int)}));
        
        mainMethGen.Emit(OpCodes.Ldloc, localDelegate2);
        mainMethGen.Emit(OpCodes.Ldc_I4, 3);
        mainMethGen.Emit(OpCodes.Ldc_I4, 5);
        mainMethGen.Emit(OpCodes.Callvirt, delegateType.GetMethod("Invoke", new Type[]{typeof(int), typeof(int)}));
        mainMethGen.Emit(OpCodes.Call,typeof(System.Console).GetMethod("WriteLine", new Type[]{typeof(int)}));
        
        mainMethGen.Emit(OpCodes.Ldloc, localDelegate2);
        mainMethGen.Emit(OpCodes.Stloc, localDelegate1);
        
        mainMethGen.Emit(OpCodes.Ldloc, localDelegate1);
        mainMethGen.Emit(OpCodes.Ldc_I4, 3);
        mainMethGen.Emit(OpCodes.Ldc_I4, 5);
        mainMethGen.Emit(OpCodes.Callvirt, delegateType.GetMethod("Invoke", new Type[]{typeof(int), typeof(int)}));
        mainMethGen.Emit(OpCodes.Call,typeof(System.Console).GetMethod("WriteLine", new Type[]{typeof(int)}));
#endregion
*/        

        LocalBuilder outerClass = mainMethGen.DeclareLocal(sumFuncType);
        LocalBuilder localDelegate1 = mainMethGen.DeclareLocal(delIntIntType);
        LocalBuilder localDelegate2 = mainMethGen.DeclareLocal(delegateType);
        LocalBuilder aInteger = mainMethGen.DeclareLocal(typeof(int));
        mainMethGen.Emit(OpCodes.Newobj, sumFuncType.GetConstructor(Type.EmptyTypes));
        mainMethGen.Emit(OpCodes.Stloc, outerClass);
        mainMethGen.Emit(OpCodes.Ldloc, outerClass);
        mainMethGen.Emit(OpCodes.Ldc_I4, 5);
        mainMethGen.Emit(OpCodes.Stfld, sumFuncField);
        //mainMethGen.Emit(OpCodes.Stfld, sumFunction.GetField("x"));

        mainMethGen.Emit(OpCodes.Ldloc, outerClass);
        mainMethGen.Emit(OpCodes.Ldftn, sumFuncGenDelegate);
        //mainMethGen.Emit(OpCodes.Ldftn, sumFunction.GetMethod("F1"));

        mainMethGen.Emit(OpCodes.Newobj, delIntIntType.GetConstructor(Type.EmptyTypes));
        mainMethGen.Emit(OpCodes.Stloc, localDelegate1);

        mainMethGen.Emit(OpCodes.Ldnull);   // ???????????????????????????
        mainMethGen.Emit(OpCodes.Ldftn, sumMeth);
        mainMethGen.Emit(OpCodes.Newobj, delegateType.GetConstructor(Type.EmptyTypes));
        mainMethGen.Emit(OpCodes.Stsfld, fFirst);
        mainMethGen.Emit(OpCodes.Ldsfld, fFirst);
        mainMethGen.Emit(OpCodes.Stloc, localDelegate2);

        mainMethGen.Emit(OpCodes.Ldloc, localDelegate1);
        mainMethGen.Emit(OpCodes.Ldc_I4, 8);
        mainMethGen.Emit(OpCodes.Ldc_I4, 5);
        mainMethGen.Emit(OpCodes.Callvirt, delIntIntType.GetMethod("Invoke", new Type[]{typeof(int), typeof(int)}));
        mainMethGen.Emit(OpCodes.Ldc_I4, 3);
        mainMethGen.Emit(OpCodes.Ldc_I4, 5);
        mainMethGen.Emit(OpCodes.Callvirt, delegateType.GetMethod("Invoke", new Type[]{typeof(int), typeof(int)}));
        mainMethGen.Emit(OpCodes.Stloc, aInteger);
        mainMethGen.Emit(OpCodes.Ldloc, aInteger);
        mainMethGen.Emit(OpCodes.Call,typeof(System.Console).GetMethod("WriteLine", new Type[]{typeof(int)}));


        mainMethGen.Emit(OpCodes.Ret);
    // end of Main method

        // set assembly entry point
        asmBuilder.SetEntryPoint(mainMeth);
        // create program type
        programType.CreateType();

        asmBuilder.Save(name + ".exe");
    }
}