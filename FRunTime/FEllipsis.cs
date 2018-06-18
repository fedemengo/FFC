namespace FFC.FRunTime
{
    public class FEllipsis : FRTType, FIterable<FInteger>
    {
        public FInteger Begin {get; set;}
        public FInteger End {get; set;}

        class IntIterator : FIterator<FInteger>
        {
            private FEllipsis el;
            private int current;
            public IntIterator(FEllipsis el)
            {
                this.el = el;
                current = el.Begin.Value;
            }
            public bool MoveNext()
            {
                if(current == el.End.Value) return false;
                //Moves one step closer to End
                current += current < el.End.Value ? 1 : -1;
                return true;
            }
            public FInteger GetCurrent() => new FInteger(current);
        }
        public FIterator<FInteger> getIterator()
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