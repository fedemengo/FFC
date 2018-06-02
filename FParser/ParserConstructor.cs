using QUT.Gppg;

namespace FFC.FParser
{
    internal partial class Parser : ShiftReduceParser<TValue, LexLocation>
    {
        public Parser(Scanner scanner) : base(scanner){}
    }
}