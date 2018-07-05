using System;
using System.Reflection;
using System.Reflection.Emit;
using FFC.FGen;


namespace FFC.FAST
{
    public abstract class FASTNode : FParser.TValue
    {
        /*
            inherited by anyone
         */
        public virtual void Print(int tabs) => throw new FCompilationException($"{Span} - Print not implemented for type {this}");
        static public void PrintTabs(int i)
        {
            while(i-- != 0)
                Console.Write("  ");
        }

        public virtual void Generate(ILGenerator generator, TypeBuilder currentType, ref SymbolTable st, Label exitLabel = new Label(), Label conditionLabel = new Label()) => throw new FCompilationException($"{Span} - Code generation (with SymbolTable updates) not implemented for type {this}");
        public virtual void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = new Label(), Label conditionLabel = new Label()) => throw new FCompilationException($"{Span} - Code generation not implemented for type {this}");

        protected FType ValueType {get; set;} = null;
        public FType GetValueType(SymbolTable st)
        {
            if(ValueType == null) BuildValueType(st);
            return ValueType;
        }
        public virtual void BuildValueType(SymbolTable st) => throw new FCompilationException($"{Span} - BuildType not implemented for {this}");    }
}