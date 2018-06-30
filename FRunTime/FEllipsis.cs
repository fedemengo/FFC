using System;
namespace FFC.FRunTime
{
    public class FEllipsis : FRTType, FIterable<FInteger>
    {
        public FInteger Begin;
        public FInteger End;
        class IntIterator : FIterator<FInteger>
        {
            private FEllipsis range;
            private bool alreadySet = false;
            private int current;
            public IntIterator(FEllipsis ellipsis) => range = ellipsis;
            public bool MoveNext()
            {
                if(!alreadySet)
                {
                    current = range.Begin.Value;
                    alreadySet = true;
                    return true;
                }
                //check if end was already reached
                if(current == range.End.Value) return false;
                //Moves one step closer to End
                current += current < range.End.Value ? 1 : -1;
                return true;
            }
            //Make a new one so that modifying the returned value won't affect the iterator
            public FInteger GetCurrent() => new FInteger(current);
        }
        public FIterator<FInteger> GetIterator() => new IntIterator(this);
        public FEllipsis(FInteger a, FInteger b)
        {
            Begin = a;
            End = b;
        }
    }
}