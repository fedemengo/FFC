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
        public FOperator op;
        public FExpression right;
    }
    class NegativeExpression : FExpression
    {
        public FSecondary value;
    }
    class EllipsisExpression : FExpression
    {
        public FSecondary from;
        public FSecondary to;
    }
}