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
            if(header != null)
                header.Print(tabs + 1);
            body.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //we make two new lebels : now break/continue won't affect previous loops
            conditionLabel = generator.DefineLabel();
            exitLabel = generator.DefineLabel();

            //loop condition is generated inside header if possible
            if(header is WhileHeader)
                header.Generate(generator, currentType, st, exitLabel, conditionLabel);
            else if(header is ForHeader)
                header.Generate(generator, currentType, ref st, exitLabel, conditionLabel);
            else if(header is null)
                generator.MarkLabel(conditionLabel);

            body.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Br, conditionLabel);
            generator.MarkLabel(exitLabel);
        }
        public override void BuildType(SymbolTable st)
        {
            //shall test this part
            if(header is ForHeader)
            {
                var x = header as ForHeader;
                if(x.id != null)
                    st = st.Assign(x.id.name, new NameInfo(null, (x.collection.GetValueType(st) as IterableType).type));
            }
            valueType = body.GetValueType(st);
        }
    }
    public abstract class FLoopHeader : FASTNode
    {
        
    }
    public class ForHeader : FLoopHeader
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

        public override void Generate(ILGenerator generator, TypeBuilder currentType, ref SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            // Generate iterator
            FType collType = collection.GetValueType(st);
            IterableType iterableType = collType as IterableType;
            Type collValueRTType = iterableType.type.GetRunTimeType();
            
            if(iterableType == null)
                throw new NotImplementedException($"{Span} - Can't iterate on {collType}");
            collection.Generate(generator, currentType, st, exitLabel, conditionLabel);
            if(!collType.GetRunTimeType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(FIterable<>)))
                throw new NotImplementedException($"{Span} - {collType.GetRunTimeType().Name} is not FIterable {iterableType.type.GetRunTimeType().Name}");
            generator.Emit(OpCodes.Callvirt, collType.GetRunTimeType().GetMethod("GetIterator"));
            
            // Create lb for iterator
            LocalBuilder it = generator.DeclareLocal(typeof(FIterator<>).MakeGenericType(collValueRTType));
            Generator.EmitStore(generator, it);
            // If header has id, loads it
            if(id != null)
            {
                //assign new Local to id in SymbolTable 
                st = st.Assign(id.name, new NameInfo(generator.DeclareLocal(collValueRTType), iterableType.type));
            }

            // Advance iterator
            generator.MarkLabel(conditionLabel); // Has to repeat from here
            Generator.EmitLoad(generator, it);
            generator.Emit(OpCodes.Callvirt, typeof(FIterator<>).MakeGenericType(collValueRTType).GetMethod("MoveNext"));
            //If false (reached it end) exit from loop
            generator.Emit(OpCodes.Brfalse, exitLabel);
            //if id was set, updates it
            if(id != null)
            {
                Generator.EmitLoad(generator, it);
                //get value
                generator.Emit(OpCodes.Callvirt, typeof(FIterator<>).MakeGenericType(collValueRTType).GetMethod("GetCurrent"));
                //assign it to "iterating" variable
                Generator.EmitStore(generator, st.Find(id.name).Builder);
            }
        }
    }
    public class WhileHeader : FLoopHeader
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

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.MarkLabel(conditionLabel); //While repeats from condition check
            if(condition.GetValueType(st).GetRunTimeType() != typeof(FBoolean))
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.GetValueType(st)}");
            condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            generator.Emit(OpCodes.Brfalse, exitLabel);
        }
    }
}