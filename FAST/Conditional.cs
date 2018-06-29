using System;
using System.Collections.Generic;
using FFC.FParser;
using FFC.FRunTime;
using System.Reflection.Emit;
using System.Reflection;
using FFC.FGen;

namespace FFC.FAST
{
    public class Conditional : FPrimary
    {
        public FExpression condition;
        public FExpression ifTrue;
        public FExpression ifFalse;
        public Conditional(FExpression condition, FExpression ifTrue, FExpression ifFalse, TextSpan span)
        {
            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
            this.Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Conditional expression");
            condition.Print(tabs + 1);
            ifTrue.Print(tabs + 1);
            ifFalse.Print(tabs + 1);
        }

        public override void BuildType(SymbolTable st) 
        {
            var t = ifTrue.GetValueType(st);
            var f = ifFalse.GetValueType(st);
            //checks for recursive functions
            if(t == null) valueType = f;
            else if (f == null) valueType = t;
            else if(FType.SameType(t, f))
                valueType = ifTrue.GetValueType(st);
            else
                throw new NotImplementedException($"{Span} - Different type in conditional expression");
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(condition.GetValueType(st) is BooleanType == false)
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.GetValueType(st)}");
            condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            Label falseBranch = generator.DefineLabel();
            Label exitBranch = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse, falseBranch);
            ifTrue.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Br, exitBranch);
            generator.MarkLabel(falseBranch);
            ifFalse.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.MarkLabel(exitBranch);
        }
    }
}