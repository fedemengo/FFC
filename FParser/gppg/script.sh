# generate original parser
wine FParser/gppg/gppg.exe FParser/gppg/grammar.y > FParser/Parser.cs

# Refactors error to ERROR
cat FParser/Parser.cs | sed -e 's/error/ERROR/' > tmp.parser

# generate ETokens
echo "using System;" > FLexer/ETokens.cs
echo "" >> FLexer/ETokens.cs
echo "namespace FFC.FLexer" >> FLexer/ETokens.cs
echo "{" >> FLexer/ETokens.cs
# matches the tokens and prints them
awk '/[A-Z_]*=[0-9]*,/{print $0}' tmp.parser >> FLexer/ETokens.cs
# finishes the file
echo "}" >> FLexer/ETokens.cs
# remove tokens from tmp.parser and prints in Parser.cs
cat tmp.parser | tr '\n' '^' | tr '\r' '~' | sed -e 's/internal enum ETokens {.*[0-9]};~^~^//' | tr '^' '\n' | tr '~' '\r' > FParser/Parser.cs

# remove tmp.parser
rm tmp.parser