using System.Reflection.Emit;
using System.Reflection;
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

        public virtual void Generate(ILGenerator generator, SymbolTable st)
        {
            throw new System.NotImplementedException($"{Span} - Code generation not implemented for type {GetType().Name}");
        }
    }
}