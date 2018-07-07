using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FPair<K, V> : FRTType
    {
        public K Key;
        public V Value;
        public FPair(KeyValuePair<K,V> p)
        {
            Key = p.Key;
            Value = p.Value;
        }
    }
    public class FMap<K, V> : FRTType, FIterable<FPair<K, V>> where K: FRTType, IComparable<K> where V: FRTType 
    {
        public Dictionary<K, V> Values;
        public FMap() => Values = new Dictionary<K, V>();
        //chain constructor approach
        public FMap(FMap<K, V> a, FPair<K,V> p)
        {
            Values = a.Values;
            Values.Add(p.Key, p.Value);
        }
        public V this[K key]
        {
            get => Values[key];
            set => Values[key] = value;
        }
        public override string ToString()
        {
            string ans = "{";
            foreach(var v in Values)
                ans += v.Key.ToString() + " : " + v.Value.ToString() + ", ";
            //remove last ", "
            if(ans.Length >= 4) ans = ans.Remove(ans.Length - 2);
            ans += "}";
            return ans;
        }
        public uint Length => (uint)Values.Count;
        class KVIterator : FIterator<FPair<K, V>>
        {
            private Dictionary<K, V>.Enumerator iterator;
            public KVIterator(FMap<K, V> collection) => iterator =  collection.Values.GetEnumerator();
            public bool MoveNext() => iterator.MoveNext();
            public FPair<K, V> GetCurrent() => new FPair<K, V>(iterator.Current);
        }
        public FIterator<FPair<K, V>> GetIterator() => new KVIterator(this);
        public void Assign(FMap<K, V> other) => Values = other.Values;

    }
}