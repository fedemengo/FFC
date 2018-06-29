namespace FFC.FRunTime
{
    public interface FIterable<T>
    {
        FIterator<T> GetIterator();
    }
    public interface FIterator<T>
    {
        bool MoveNext();
        T GetCurrent();
    }
}