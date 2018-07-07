using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FArray<V> : FRTType, FIterable<V>
    {
        public List<V> Values;
        public FArray()
        {
            if(typeof(V).IsSubclassOf(typeof(FRTType)) == false && typeof(V).IsSubclassOf(typeof(Delegate)) == false)
                throw new ArrayTypeMismatchException($"Cannot create FArray of type {typeof(V).Name}");  
            Values = new List<V>();
        }
        public FArray(V v) : this() => Values.Add(v);
        //even if we create N FArray to create a (non immutable) FArray of size N,
        //GC should save us from wasted memory
        public FArray(FArray<V> a, V v)
        {
            //not using this to avoid creating useless list
            if(typeof(V).IsSubclassOf(typeof(FRTType)) == false && typeof(V).IsSubclassOf(typeof(Delegate)) == false)
                throw new ArrayTypeMismatchException($"Cannot create FArray of type {typeof(V).Name}");              
            Values = a.Values;
            Values.Add(v);
        }
        // concatenation
        public FArray(FArray<V> a1, FArray<V> a2)
        {
            //not even remotely efficient
            Values = new List<V>();
            foreach(V v in a1.Values)
                Values.Add(v);
            foreach(V v in a2.Values)
                Values.Add(v);
        }
        public static FArray<V> operator+(FArray<V> a1, FArray<V> a2) => new FArray<V>(a1, a2);
        public static FArray<V> operator+(FArray<V> a, V v) => new FArray<V>(a, new FArray<V>(v));
        public V this[FInteger i]
        {
            get => Values[i.Value];
            set => Values[i.Value] = value;
        }
        public override string ToString()
        {
            string ans = "{";
            foreach(V v in Values)
                ans += v.ToString() + ", ";
            //remove last ", "
            if(ans.Length >= 4) ans = ans.Remove(ans.Length - 2);
            ans += "}";
            return ans;
        }
        public uint Length => (uint)Values.Count;
        class VIterator : FIterator<V>
        {
            private FArray<V> collection;
            private int index = -1;
            public VIterator (FArray<V> coll) => collection = coll;
            public V GetCurrent() => collection.Values[index];
            public bool MoveNext() => ++index < collection.Length;
        }
        public FIterator<V> GetIterator() => new VIterator(this);
        public void Assign(FArray<V> other) => Values = other.Values;

    }
}