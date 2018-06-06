wine FParser/gppg/gppg.exe FParser/gppg/grammar.y > FParser/Parser.cs
bash FParser/gppg/fix_parser.sh > tmp.cs
mv tmp.cs FParser/Parser.cs