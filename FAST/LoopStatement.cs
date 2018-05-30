using System.Collections.Generic;
namespace FAST
{
    class LoopStatement : FStatement
    {
        public FLoopHeader header;
        public List<FStatement> body;
    }
    abstract class FLoopHeader : FASTNode
    {

    }
    class ForHeader : FLoopHeader
    {
        public Identifier id;
        public FExpression collection;
    }
    class WhileHeader : FLoopHeader
    {
        public FExpression condition;
    }
}