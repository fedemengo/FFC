using System;
using System.Collections.Generic;
using FFC.FParser;
using FFC.FRunTime;
using System.Reflection.Emit;
using System.Reflection;
using FFC.FGen;

namespace FFC.FAST
{
    class Conditional : FPrimary
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

        public override void BuildType() 
        {
            if(ifTrue.ValueType.GetRunTimeType() == ifFalse.ValueType.GetRunTimeType())
                ValueType = ifTrue.ValueType;
            else
                throw new NotImplementedException($"{Span} - Different type in conditional expression");
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            if(condition.ValueType.GetRunTimeType() != typeof(FBoolean))
            {
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.ValueType}");
            }
            condition.Generate(generator, st);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            Label falseBranch = generator.DefineLabel();
            Label exitBranch = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse, falseBranch);
            ifTrue.Generate(generator, st);
            generator.Emit(OpCodes.Br, exitBranch);
            generator.MarkLabel(falseBranch);
            ifFalse.Generate(generator, st);
            generator.MarkLabel(exitBranch);
        }
    }
}