using QUT.Gppg;
using FFC.FGen;

namespace FFC.FParser
{
    internal partial class Parser : ShiftReduceParser<TValue, LexLocation>
    {
        public Parser(Scanner scanner) : base(scanner){}
    }
}