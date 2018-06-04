namespace FFC.FAST
{
    abstract class FASTNode : FParser.TValue
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
    }
}