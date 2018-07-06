using System;
using System.IO;
using FFC.FGen;

namespace FFC.FLexer
{
    class SourceReader
    {
        private string[] buffer;
        private bool isFinished;
        private Position position;
        public Position GetPosition() => new Position(position);
        
        public SourceReader(string filePath)
        {
            buffer = File.ReadAllLines(filePath);
            isFinished = false;
            position = new Position();
        }
        
        public void SkipLine()
        {
            char x = GetChar();
            Advance();
            if(x == '\n' || x == '\0')
                return;
            SkipLine();
        }

        
        public bool Empty() => isFinished;
        
        public void SkipBlank()
        {
            char x = GetChar();
            Advance();
            if(x == ' ' || x == '\n' || x == '\t' || x == '\0')
                return;
            SkipBlank();
        }
        
        public char GetChar()
        {
            if(isFinished)
                return '\0';
            if(position.Column == buffer[position.Row].Length)
                return '\n';
            return buffer[position.Row][position.Column];
        }
        
        public void Advance()
        {
            if(position.Column == buffer[position.Row].Length)
                position.NextLine();
            else
                position.NextChar();

            if(position.Row == buffer.Length)
                isFinished = true;
        }
    }
}