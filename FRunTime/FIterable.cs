namespace FFC.FRunTime
{
    public interface FIterable<T>  where T : class
    {
        FIterator<T> GetIterator();
    }
    public interface FIterator<T> where T : class
    {
        bool MoveNext();
        T GetCurrent();
    }
}