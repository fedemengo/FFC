//#define DBG_TOKEN

using System.Collections.Generic;
using System;
using FFC.FParser;using FFC.FGen;

namespace FFC.FLexer
{
    class TokenValue : TValue
    {
        private List<object> values = new List<object>();
        public void Add(object o) => values.Add(o);
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
        public ETokens Type {get; set;}
        public TokenValue values = new TokenValue();
        //Position is in a [L, R) format
        public Position Begin {get; set;}
        public Position End {get; set;}
        public Token(ETokens tokenType, Position begin, Position end)
        {
            #if DBG_TOKEN
                Console.WriteLine("Token : " + tokenType);
            #endif
            Type = tokenType;
            Begin = begin;
            End = end;
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
            string ans = Type.ToString();
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
            return ans + $"[{Begin.ToString()}, {End.ToString()}";
        }
    }
}