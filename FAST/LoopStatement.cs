using System.Collections.Generic;
using System;
using FFC.FParser;
using System.Reflection.Emit;
using System.Reflection;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FAST
{
    class LoopStatement : FStatement
    {
        public FLoopHeader header;
        public StatementList body;

        public LoopStatement(FLoopHeader header, StatementList body, TextSpan span)
        {
            this.Span = span;
            this.header = header;
            this.body = body;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Loop statement");
            header.Print(tabs + 1);
            body.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            Label loopCondition = generator.DefineLabel();
            Label exitLabel = generator.DefineLabel();
            generator.MarkLabel(loopCondition);
            header.Generate(generator, exitLabel, st);
            body.Generate(generator, loopCondition, exitLabel, st);
            generator.Emit(OpCodes.Br, loopCondition);
            generator.MarkLabel(exitLabel);
        }
    }
    abstract class FLoopHeader : FASTNode
    {
        public virtual void Generate(ILGenerator generator, Label nextLabel, SymbolTable st)
        {
            throw new NotImplementedException($"{Span} - Generation not implemented for {GetType().Name}");
        }
    }
    class ForHeader : FLoopHeader
    {
        public Identifier id;
        public FExpression collection;

        public ForHeader(Identifier id, FExpression collection, TextSpan span)
        {
            this.Span = span;
            this.id = id;
            this.collection = collection;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("For header");
            if(id != null) id.Print(tabs + 1);
            collection.Print(tabs + 1);
        }
    }
    class WhileHeader : FLoopHeader
    {
        public FExpression condition;

        public WhileHeader(FExpression condition, TextSpan span)
        {
            this.Span = span;
            this.condition = condition;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("While header");
            condition.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, Label exitLabel, SymbolTable st)
        {
            if(condition.GetValueType(st).GetRunTimeType() != typeof(FBoolean))
            {
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.GetValueType(st)}");
            }
            condition.Generate(generator, st);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            generator.Emit(OpCodes.Brfalse, exitLabel);
        }
    }
}