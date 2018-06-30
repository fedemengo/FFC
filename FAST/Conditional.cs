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
        public FExpression Condition {get; set;}
        public FExpression IfTrue {get; set;}
        public FExpression IfFalse {get; set;}
        public Conditional(FExpression condition, FExpression ifTrue, FExpression ifFalse, TextSpan span)
        {
            Condition = condition;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Conditional expression");
            Condition.Print(tabs + 1);
            IfTrue.Print(tabs + 1);
            IfFalse.Print(tabs + 1);
        }

        public override void BuildValueType(SymbolTable st) 
        {
            var t = IfTrue.GetValueType(st);
            var f = IfFalse.GetValueType(st);
            //checks for recursive functions
            if(t == null) ValueType = f;
            else if (f == null) ValueType = t;
            else if(FType.SameType(t, f))
                ValueType = IfTrue.GetValueType(st);
            else
                throw new FCompilationException($"{Span} - Different type in conditional expression");
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(Condition.GetValueType(st) is BooleanType == false)
                throw new FCompilationException($"{Span} - Can't use conditional with {Condition.GetValueType(st)}");
            Condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("GetBool"));
            Label falseBranch = generator.DefineLabel();
            Label exitBranch = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse, falseBranch);
            IfTrue.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Br, exitBranch);
            generator.MarkLabel(falseBranch);
            IfFalse.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.MarkLabel(exitBranch);
        }
    }
}