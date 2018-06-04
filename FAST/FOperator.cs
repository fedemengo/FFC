using System.Collections.Generic;
using System;

namespace FFC.FAST
{
    abstract class FOperator : FASTNode
    {
        /*
            inherited by
            LessOperator
            LessEqualOperator
            GreaterOperator
            GreaterEqualOperator
            EqualOperator
            NotEqualOperator
            AndOperator
            OrOperator
            XorOperator
            PlusOperator
            MinusOperator
            StarOperator
            SlashOperator
        */
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this.GetType().TypeHandle);
        }

    }
    class LessOperator : FOperator
    {
        public LessOperator()
        {

        }
    }
    class LessEqualOperator : FOperator
    {
        public LessEqualOperator()
        {

        }
    }
    class GreaterOperator : FOperator
    {
        public GreaterOperator()
        {

        }
    }
    class GreaterEqualOperator : FOperator
    {
        public GreaterEqualOperator()
        {

        }
    }
    class EqualOperator : FOperator
    {
        public EqualOperator()
        {

        }
    }
    class NotEqualOperator : FOperator
    {
        public NotEqualOperator()
        {

        }
    }
    class AndOperator : FOperator
    {
        public AndOperator()
        {

        }
    }
    class OrOperator : FOperator
    {
        public OrOperator()
        {

        }
    }
    class XorOperator : FOperator
    {
        public XorOperator()
        {

        }
    }
    class PlusOperator : FOperator
    {
        public PlusOperator()
        {

        }
    }
    class MinusOperator : FOperator
    {
        public MinusOperator()
        {

        }
    }
    class StarOperator : FOperator
    {
        public StarOperator()
        {

        }
    }
    class SlashOperator : FOperator
    {
        public SlashOperator()
        {

        }
    }
}