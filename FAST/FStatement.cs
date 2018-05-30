using System.Collections.Generic;

namespace FAST
{
    abstract class FStatement : FASTNode
    {
        /*
            inherited by
            FunctionCallStatement
            AssignmentStatemt
            DeclStm
            IfStm
            LoopStm
            ReturnStm
            BreakStm
            ContinueStm
            PrintStm
         */
    }
    class FunctionCallStatement : FStatement
    {
        public FunctionCall function;
    }
    class AssignmentStatemt : FStatement
    {
        public FSecondary left;
        public FExpression right;
    }
    class DeclarationStatement : FStatement
    {
        public Identifier id;
        public FType type;
        public FExpression expr;
    }
    class IfStatement : FStatement
    {
        public FExpression condition;
        public List<FStatement> ifTrue;
        public List<FStatement> ifFalse;
    }
    class ReturnStatement : FStatement
    {
        public FExpression value;
    }
    class BreakStatement : FStatement
    {

    }
    class ContinueStatement : FStatement
    {

    }
    class PrintStatement : FStatement
    {
        List<FExpression> toPrint;
    }
}