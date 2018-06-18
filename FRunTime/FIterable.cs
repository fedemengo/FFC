namespace FFC.FRunTime
{
    public interface FIterable<T>  where T : FRTType
    {
        FIterator<T> GetIterator();
    }
    public interface FIterator<T> where T : FRTType
    {
        bool MoveNext();
        T GetCurrent();
    }
}