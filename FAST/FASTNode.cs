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
        public virtual void Print(int tabs) => throw new System.NotImplementedException($"{Span} - Print not implemented for type {GetType().Name}");
        static public void PrintTabs(int i)
        {
            while(i-- != 0)
                System.Console.Write("  ");
        }

        public virtual void Generate(ILGenerator generator, TypeBuilder currentType, ref SymbolTable st, Label exitLabel = new Label(), Label conditionLabel = new Label()) => throw new System.NotImplementedException($"{Span} - Code generation (with SymbolTable updates) not implemented for type {GetType().Name}");
public virtual void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = new Label(), Label conditionLabel = new Label()) => throw new System.NotImplementedException($"{Span} - Code generation not implemented for type {GetType().Name}");
        //if not specified, you just skip on using label for jumps
        //public virtual void Generate(ILGenerator generator, TypeBuilder currentType, Label conditionLabel, Label exitLabel, SymbolTable st) => throw new System.NotImplementedException($"{Span} - Code generation (with conditional jumps) not implemented for type {GetType().Name}");

        protected FType ValueType {get; set;} = null;
        public FType GetValueType(SymbolTable st)
        {
            if(ValueType == null) BuildValueType(st);
            return ValueType;
        }
        public virtual void BuildValueType(SymbolTable st) => throw new NotImplementedException($"{Span} - BuildType not implemented for {this.GetType().Name}");    }
}