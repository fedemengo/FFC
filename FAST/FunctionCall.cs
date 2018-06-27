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
            toCall.Generate(generator, currentType, st, exitLabel, conditionLabel);
            List<Type> paramTypes = new List<Type>();
            foreach(FExpression exprs in exprs.expressions)
            {
                paramTypes.Add(exprs.GetValueType(st).GetRunTimeType());
                exprs.Generate(generator, currentType, st);
            }
            TypeBuilder funcType = Generator.FunctionTypes[toCall.GetValueType(st) as FunctionType];
            generator.Emit(OpCodes.Callvirt, funcType.GetMethod("Invoke", paramTypes.ToArray()));
        }
    }
}
