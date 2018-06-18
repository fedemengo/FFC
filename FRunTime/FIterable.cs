namespace FFC.FRunTime
{
    public interface FIterable<T>  where T : FRTType
    {
        FIterator<T> getIterator();
    }
    public interface FIterator<T> where T : FRTType
    {
        bool MoveNext();
        T GetCurrent();
    }
}