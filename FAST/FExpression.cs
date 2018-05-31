using System.Collections.Generic;

namespace FFC.FAST
{
    abstract class FExpression : FASTNode
    {
        /*
            inherited by
                BinOpExpr
                NegExpr
                EllipsisExpr
                FSecondary
        */
    }
    class BinaryOperatorExpression : FExpression
    {
        public FSecondary left;
        public FOperator binOperator;
        public FExpression right;
        public BinaryOperatorExpression(FSecondary left, FOperator binOperator, FExpression right)
        {
            this.left = left;
            this.binOperator = binOperator;
            this.right = right;
        }
    }
    class NegativeExpression : FExpression
    {
        public FSecondary value;
        public NegativeExpression(FSecondary value)
        {
            this.value = value;
        }
    }
    class EllipsisExpression : FExpression
    {
        public FSecondary from;
        public FSecondary to;
        public EllipsisExpression(FSecondary from, FSecondary to)
        {
            this.from = from;
            this.to = to;
        }
    }
}