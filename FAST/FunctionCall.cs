using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FGen;

namespace FFC.FAST
{
    public class FunctionCall : FSecondary
    {
        public FSecondary toCall;
        public ExpressionList exprs;
        public FunctionCall(FSecondary toCall, ExpressionList exprs, TextSpan span)
        {
            this.Span = span;
            this.toCall = toCall;
            this.exprs = exprs;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Function call");
            toCall.Print(tabs + 1);
            exprs.Print(tabs + 1);
        }
        public override void BuildType(SymbolTable st)
        {
            var t = toCall.GetValueType(st);
            if(t is null) return;
            else if(t is FunctionType) valueType = (t as FunctionType).returnType;
            else throw new NotImplementedException($"{Span} - Can't call function on {toCall.GetValueType(st)}.");
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(toCall.GetValueType(st) is FunctionType == false)
                throw new NotImplementedException($"{Span} - Can't call function on {toCall.GetValueType(st)}.");

            FunctionType funcType = toCall.GetValueType(st) as FunctionType;
            TypeBuilder funcTypeBuilder = Generator.FunctionTypes[funcType];

            if(funcType.paramTypes.types.Count != exprs.expressions.Count)
                throw new NotImplementedException($"{Span} - Parameter count mismatch on {toCall.GetValueType(st)}.");

            toCall.Generate(generator, currentType, st, exitLabel, conditionLabel);
            List<Type> paramTypes = new List<Type>();
            for(int i=0; i<exprs.expressions.Count; ++i)
            {
                FType exprFType = exprs.expressions[i].GetValueType(st);

                if(!FType.SameType(funcType.paramTypes.types[i], exprFType))
                    throw new NotImplementedException($"{Span} - Parameter {i} should be {funcType.paramTypes.types[i].ToString()} instead of {exprFType.ToString()}.");
                paramTypes.Add(exprFType.GetRunTimeType());
                exprs.expressions[i].Generate(generator, currentType, st);
            }
            
            generator.Emit(OpCodes.Callvirt, funcTypeBuilder.GetMethod("Invoke", paramTypes.ToArray()));
        }
    }
}
