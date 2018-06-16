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
        abstract public void Print(int tabs);
        static public void PrintTabs(int i)
        {
            while(i-- != 0)
                System.Console.Write("  ");
        }

        public virtual void Generate(ILGenerator generator, SymbolTable st)
        {
            throw new System.NotImplementedException(this.Span + " Code generation not implemented (" + this.GetType().Name + ")");
        }
    }
}