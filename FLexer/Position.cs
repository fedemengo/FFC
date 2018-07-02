using System;
using FFC.FGen;

namespace FFC.FLexer
{
    public class Position
    {
        public int Row {get; set;}
        public int Column{get; set;}
        public void NextChar() => Column++;
        public void NextLine()
        {
            Row++;
            Column = 0;
        }
        public Tuple<int, int> GetPair() => Tuple.Create(Row, Column);
        public Position()
        {
            Row = 0;
            Column = 0;
        }
        public Position(Position copy)
        {
            Row = copy.Row;
            Column = copy.Column;
        }
        public Position(int r, int c)
        {
            Row = r;
            Column = c;
        }
        public override string ToString() =>  "[" + (Row + 1) + ", " + (Column + 1) + "]";
    }
}
