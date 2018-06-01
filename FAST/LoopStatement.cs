using System.Collections.Generic;
namespace FFC.FAST
{
    class LoopStatement : FStatement
    {
        public FLoopHeader header;
        public StatementList body;

        public LoopStatement(FLoopHeader header, StatementList body)
        {
            this.header = header;
            this.body = body;
        }
    }
    abstract class FLoopHeader : FASTNode
    {
        
    }
    class ForHeader : FLoopHeader
    {
        public Identifier id;
        public FExpression collection;

        public ForHeader(Identifier id, FExpression collection)
        {
            this.id = id;
            this.collection = collection;
        }
    }
    class WhileHeader : FLoopHeader
    {
        public FExpression condition;

        public WhileHeader(FExpression condition)
        {
            this.condition = condition;
        }
    }
}