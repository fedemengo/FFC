using System;
using System.Collections.Generic;

namespace Lexer
{
    class NFA
    {
        public KeyValuePair<char, NFANode> tail {get; set;}
        public NFANode head {get; set;}
        public Token tk {get; set;} 
        public void Concatenate(NFA other)
        {   
            head.Insert(other.tail);
            head = other.head;
        }
        public void Starify()
        {
            //create a new head
            NFANode newHead = new NFANode();
            //link current dfa to it
            head.freeLinks.Add(newHead);
            //add tail to the new head(
            newHead.Insert(tail);
            //now we are ready to assign new head and tail
            tail = new KeyValuePair<char, NFANode>('\0', newHead);
            head = newHead;
        }
        public void Alternate(NFA other)
        {
            //new ending node
            NFANode newHead = new NFANode();
            KeyValuePair<char, NFANode> headEdge = KeyValuePair.Create('\0', newHead);
            head.Insert(headEdge);
            other.head.Insert(headEdge);
            //we create two free nodes
            NFANode free1 = new NFANode();
            NFANode free2 = new NFANode();
            free1.Insert(tail);
            free2.Insert(other.tail);
            //we set tail to a new node
            tail = KeyValuePair.Create('\0', new NFANode());
            tail.Value.freeLinks.Add(free1);
            tail.Value.freeLinks.Add(free2);
            head = newHead;
        }
        public NFA(char c)
        {
            head = new NFANode();
            tail = KeyValuePair.Create(c, head);
        }
        public NFA()
        {
            head = new NFANode();
            tail = KeyValuePair.Create('\0', head);
        }
        public NFA(string sequence)
        {
            List<NFA> list = new List<NFA>();
            foreach(char c in sequence)
                list.Add(new NFA(c));
            //copy first one
            tail = list[0].tail;
            head = list[0].head;
            //concatenate the rest
            for(int i = 1; i < list.Count; i++)
                Concatenate(list[i]);
        }
        public NFA(List<char> options)
        {
            head = new NFANode();
            tail = KeyValuePair.Create('\0', new NFANode());
            foreach(char c in options)
                tail.Value.Insert(KeyValuePair.Create(c, head));
        }
    }
}
