namespace FFC.FRunTime
{
    public class FEllipsis : FRTType, FIterable<FInteger>
    {
        public FInteger Begin {get; set;}
        public FInteger End {get; set;}

        class IntIterator : FIterator<FInteger>
        {
            private FEllipsis el;
            private bool set = false;
            private int current;
            public IntIterator(FEllipsis el)
            {
                this.el = el;
            }
            public bool MoveNext()
            {
                if(!set)
                {
                    current = el.Begin.Value;
                    set = true;
                    return true;
                }
                //check if end was already reached
                if(current == el.End.Value) return false;
                //Moves one step closer to End
                current += current < el.End.Value ? 1 : -1;
                return true;
            }
            //Make a new one so that modifying the returned value won't affect the iterator
            public FInteger GetCurrent() => new FInteger(current);
        }
        public FIterator<FInteger> GetIterator()
        {
            return new IntIterator(this);
        }
        public FEllipsis(FInteger a, FInteger b)
        {
            Begin = a;
            End = b;
        }
    }
}