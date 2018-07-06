help() {
    echo "##################################################"
    echo "## Automatize all operations during development ##"
    echo "##################################################"
    echo ""
    echo "parser        Generate Parser for grammar"
    echo "build         Build project"
    echo "lib           Generate library FFC.dll"
    echo "compile FILE  Compile the source FILE"
    echo "run EXE       Run the executable EXEC"
    echo "crun FILE     Compile and run the source FILE"
    echo "clear         Remove all .exe files"
    echo "all           Compile and Run all source in 'samples/'"
    return 1
}

build() {
    echo "Building the project..."
    echo ""
    msbuild
    echo ""
}

compile() {
    echo "Compiling..."
    echo ""
    if [[ "$3" -ne "" ]]; then
        mono bin/Release/FFC.exe "$1" "$2"
    else
        mono bin/Release/FFC.exe "$1"
    fi
    echo ""
}

run() {
    echo ""
    mono "$1"
    echo ""
}

all() {
    echo ""
    for file in samples/*; do ./util.sh crun $file main; read _; done
    echo ""
}

lib() {
    echo "Generating library..."
    echo ""
    csc FRunTime/*.cs /t:library /out:FFC.dll
    echo ""
}

parser() {
    echo "Generating parser..."
    echo ""
    # generate original parser
    mono FParser/gppg/gppg.exe FParser/gppg/grammar.y > FParser/Parser.cs
    echo "Cleaning up..."
    PATT=".*[a-zA-Z_]*=[0-9]*"
    PATT1=".*[a-zA-Z_]*=[0-9]*,.*"
    PATT2=".*[a-zA-Z_]*=[0-9]*};.*"
    # Refactors error to ERROR
    sed -i '' -e 's/error/ERROR/' FParser/Parser.cs 2>/dev/null
    # generate ETokens
    echo $'using System;\n\nnamespace FFC.FLexer\n{' > FLexer/ETokens.cs
    # matches the tokens and prints them
    awk -v pattern="(${PATT},.*)|(${PATT}};.*)" '$0~pattern {print}' FParser/Parser.cs >> FLexer/ETokens.cs
    # finishes the file
    echo "}" >> FLexer/ETokens.cs
    # remove tokens Parser.cs
    sed -i '' -e "6,11d;/\(\(${PATT},.*\)\)/d;/\(${PATT}};.*\)/d" FParser/Parser.cs 2>/dev/null
    echo ""
}

perform_task() {
    min=1
    max=1
    [[ "$1" =~ ^"run"$ ]] && min=2 && max=2
    [[ "$1" =~ ^"compile"$|^"crun"$ ]] && min=2 && max=3
    
    if [[ "$#" -lt "$min" ]]; then
        echo "Too few argument"
        return 1
    elif [[ "$#" -gt "$max" ]]; then 
        echo "Too many arguments"
        return 1
    fi
    case $1 in
    "help")
     	help;;
    "build")
        build;;
    "compile")
        compile $2 $3;;
    "run")
        run $2;;
    "all")
        all;;
    "crun")
        compile $2 $3
        if [ "$?" == "0" ]; then
            run "$(echo "$2" | awk '{n=split($0,v,"/"); print v[n]}' | awk '{split($0,v,"."); print v[1]}').exe" 2>/dev/null
        else
            retun 1;
        fi;;
    "lib")
        lib;;
    "clear")
        echo "Remove all *.exe..."
        rm *.exe*;;
    "parser")
        parser;;
    *)
        echo "Unkwon argument"
        echo "Use 'help' for more information"
        return 1;;
    esac
}

perform_task "$@"

if [ "$?" == "0" ]; then
    echo "Task completed!"
fi
