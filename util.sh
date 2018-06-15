perform_task() {
    arg=1
    [[ ( "$1" =~ ^"compile"$|^"run"$ ) ]] && arg=2
    
    if [[ ( "$#" -lt "$arg" ) ]]; then
        echo "Too few argument"
        return 1
    elif [[ ( "$#" -gt "$arg" ) ]]; then 
        echo "Too many arguments"
        return 1
    fi
    case $1 in
    "help")
        echo ""
        echo "parser - Generate Parser for grammar"
        echo "build - Build project"
        echo "lib - Generate FFC.dll"
        echo "compile FILE - Compile the source FILE"
        echo "run EXE - Run the executable EXEC"
        return 1
        ;;
    "build")
        echo "Building the project..."
        echo ""
        msbuild
        echo ""
        return 0
        ;;
    "compile")
        echo "Compiling..."
        echo ""
        mono bin/Release/FFC.exe "$2"
        echo ""
        return 0
        ;;
    "run")
        echo ""
        mono "$2"
        echo ""
        return 0
        ;;
    "lib")
        echo "Generating library..."
        echo ""
        csc FRunTime/*.cs /t:library /out:FFC.dll
        echo ""
        return 0
        ;;
    "clear")
        echo "Remove all *.exe..."
        rm *.exe*
        return 0
        ;;
    "parser")
        echo "Generating parser..."
        echo ""
        # generate original parser
        mono FParser/gppg/gppg.exe FParser/gppg/grammar.y > FParser/Parser.cs
        
        echo "Cleaning up..."
        PATT1="internal enum Etokens {"
        PATT2=".*[a-zA-Z_]*=[0-9]*,.*"
        PATT3=".*[a-zA-Z_]*=[0-9]*};.*"
        # Refactors error to ERROR
        sed -i '' -e 's/error/ERROR/' FParser/Parser.cs
        # generate ETokens
        echo $'using System;\n\nnamespace FFC.FLexer\n{' > FLexer/ETokens.cs
        # matches the tokens and prints them
        awk '/(internal enum Etokens {)|([a-zA-Z_]*=[0-9]*,)|([a-zA-Z_]*=[0-9]*\};)/{print $0}' FParser/Parser.cs >> FLexer/ETokens.cs
        # finishes the file
        echo "}" >> FLexer/ETokens.cs
        # remove tokens Parser.cs
        sed -i '' -e "6,8d;10,11d;/\(\($PATT1\)\)/d;/\(\($PATT2\)\)/d;/\($PATT3\)/d" FParser/Parser.cs
        echo ""
        return 0
        ;;
    *)
        echo "Unkwon argument"
        echo "Use 'help' for more information"
        return 1
        ;;
    esac
}

perform_task "$@"

if [ "$?" == "0" ]; then
    echo "Task completed!"
fi
