using System;
using System.Collections;
using System.Collections.Generic;

namespace testLex
{
    public class RegexItem : IEnumerable<char>
    {
        public List<char> chars = new List<char>();
        public bool isEnding = false;
        public bool isStar = false;
        public RegexItem loop = null;

        public RegexItem(List<Tuple<char, char>> list, bool star = false, bool ending = false)
        {
            foreach(var tuple in list)
            {
                for(char s = tuple.Item1; s <= tuple.Item2; s++)
                {
                    chars.Add(s);
                }
            }

            isEnding = ending;
            isStar = star;
        }

        public IEnumerator<char> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return chars.GetEnumerator();
        }
    }

    public class Regex : IEnumerable<RegexItem>
    {
        private List<RegexItem> itemList = new List<RegexItem>();
        
        public void Add(RegexItem x)
        {
            itemList.Add(x);
        }

        public IEnumerator<RegexItem> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return itemList.GetEnumerator();
        }
    }
}