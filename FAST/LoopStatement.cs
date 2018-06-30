using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FAST
{
    public class LoopStatement : FStatement
    {
        public FLoopHeader Header {get; set;}
        public StatementList Body {get; set;}

        public LoopStatement(FLoopHeader header, StatementList body, TextSpan span)
        {
            Header = header;
            Body = body;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Loop statement");
            if(Header != null)
                Header.Print(tabs + 1);
            Body.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //we make two new lebels : now break/continue won't affect previous loops
            conditionLabel = generator.DefineLabel();
            exitLabel = generator.DefineLabel();

            //loop condition is generated inside header if possible
            if(Header is WhileHeader)
                Header.Generate(generator, currentType, st, exitLabel, conditionLabel);
            else if(Header is ForHeader)
                Header.Generate(generator, currentType, ref st, exitLabel, conditionLabel);
            else if(Header is null)
                generator.MarkLabel(conditionLabel);

            Body.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Br, conditionLabel);
            generator.MarkLabel(exitLabel);
        }
        public override void BuildValueType(SymbolTable st)
        {
            //shall test this part
            if(Header is ForHeader)
            {
                var x = Header as ForHeader;
                if(x.Id != null)
                    st = st.Assign(x.Id.Name, new NameInfo(null, (x.Collection.GetValueType(st) as IterableType).Type));
            }
            ValueType = Body.GetValueType(st);
        }
    }
    public abstract class FLoopHeader : FASTNode
    {
    }
    public class ForHeader : FLoopHeader
    {
        public Identifier Id {get; set;}
        public FExpression Collection {get; set;}

        public ForHeader(Identifier id, FExpression collection, TextSpan span)
        {
            Id = id;
            Collection = collection;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("For header");
            if(Id != null) Id.Print(tabs + 1);
            Collection.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, ref SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            // Generate iterator
            FType collType = Collection.GetValueType(st);
            IterableType iterableType = collType as IterableType;
            Type collValueRTType = iterableType.Type.GetRunTimeType();
            
            if(iterableType == null)
                throw new FCompilationException($"{Span} - Can't iterate on {collType}");
            Collection.Generate(generator, currentType, st, exitLabel, conditionLabel);
            if(!collType.GetRunTimeType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(FIterable<>)))
                throw new FCompilationException($"{Span} - {collType.GetRunTimeType().Name} is not FIterable {iterableType.Type.GetRunTimeType().Name}");
            generator.Emit(OpCodes.Callvirt, collType.GetRunTimeType().GetMethod("GetIterator"));
            
            // Create lb for iterator
            LocalBuilder it = generator.DeclareLocal(typeof(FIterator<>).MakeGenericType(collValueRTType));
            Generator.EmitStore(generator, it);
            // If header has id, loads it
            if(Id != null)
            {
                //assign new Local to id in SymbolTable 
                st = st.Assign(Id.Name, new NameInfo(generator.DeclareLocal(collValueRTType), iterableType.Type));
            }

            // Advance iterator
            generator.MarkLabel(conditionLabel); // Has to repeat from here
            Generator.EmitLoad(generator, it);
            generator.Emit(OpCodes.Callvirt, typeof(FIterator<>).MakeGenericType(collValueRTType).GetMethod("MoveNext"));
            //If false (reached it end) exit from loop
            generator.Emit(OpCodes.Brfalse, exitLabel);
            //if id was set, updates it
            if(Id != null)
            {
                Generator.EmitLoad(generator, it);
                //get value
                generator.Emit(OpCodes.Callvirt, typeof(FIterator<>).MakeGenericType(collValueRTType).GetMethod("GetCurrent"));
                //assign it to "iterating" variable
                Generator.EmitStore(generator, st.Find(Id.Name).Builder);
            }
        }
    }
    public class WhileHeader : FLoopHeader
    {
        public FExpression Condition {get; set;}

        public WhileHeader(FExpression condition, TextSpan span)
        {
            Condition = condition;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("While header");
            Condition.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.MarkLabel(conditionLabel); //While repeats from condition check
            if(Condition.GetValueType(st) is BooleanType == false)
                throw new FCompilationException($"{Span} - Can't use conditional with {Condition.GetValueType(st)}");
            Condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            generator.Emit(OpCodes.Brfalse, exitLabel);
        }
    }
}