//#define DBG_TOKEN

using System.Collections.Generic;
using System;
using FFC.FParser;
namespace FFC.FLexer
{
    class TokenValue : TValue
    {
        //wraps around list
        private List<object> values = new List<object>();
        public void Add(object o)
        {
            values.Add(o);
        }
        public int Count
        {
            get => values.Count;
        }
        public object this[int i]
        {
            get => values[i];
            set => values[i] = value;
        }

    }
    class Token
    {
        public ETokens type{get; set;}
        public TokenValue values = new TokenValue();
        //Position is in a [L, R) format
        public Position begin, end;
        public Token(ETokens tokenType, Position begin, Position end)
        {
            #if DBG_TOKEN
                Console.WriteLine("Token : " + tokenType);
            #endif
            this.type = tokenType;
            this.begin = begin;
            this.end = end;
            values.Span = new TextSpan(begin, end);
        }

        public Token(ETokens tokenType, List<object> objs, Position begin, Position end) : this(tokenType, begin, end)
        {
            foreach(object o in objs)
                values.Add(o);
            #if DBG_TOKEN
                Console.Write("\t");
                foreach(object o in objs)
                    Console.Write(o.ToString());
                Console.WriteLine();
            #endif
        }

        override public string ToString()
        {
            string ans = type.ToString();
            for(int i = 0; i < values.Count; i++)
            {
                if(i == 0)
                    ans += "(";
                else
                    ans += ", ";
                ans += values[i].ToString();
                if(i == values.Count - 1)
                    ans += ")";
            }
            ans += $"[{begin.ToString()}, {end.ToString()}";
            return ans;
        }
    }
}