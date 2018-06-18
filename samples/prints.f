//currently, assignements are not runnable because\
    they leave stuff in the stack which must be empty\
    at the end of each 'il function'

main is func() do
    print "basic int";
    print 3;
    print "";

    print "int expressions";
    print 3 * 2;
    print "";

    print "other types";
    print 1.0i-2.0;
    print "";

    print "int arrays";
    print [1];
    print [3, 4];
    print "";

    print "any type arrays";
    print [3 + 4 - (1 * 2 - 7 % 3)];
    print [2.4];
    print [[1, 2], [3, 4]];
    print "";

    print "arrays concatenation";
    print [3] + [3];
    print [[1, 2], [3, 4]] + [[5, 6], [7, 8]];
    x is [[[1]]] + [[[1]]] + [[[1, 2],[3],[4, 5, 6]]];
    print x;
    for y in x loop print y; end
    print "";

    print "arrays additions";
    print [3] + 4;
    print [1,3] + ([1, 2] + 4);
    print [1.3] + 2.4 + 2.3;
    print [[1]] + [1];
    print [[1]] + [0] + [[1], [0]] + [1];
    print "";

    print "conditional expressions";
    print if !true | false then true else false end;
    print if 3 > 1 then "3 > 1" else "3 <= 1" end;
    print if 4 > 9 then "4 > 9" else "4 <= 9" end;
    print "";

    print "if statements";
    //go to if
    if true then
        print 1, "first condition";
    else if true then
        print "2 : first condition";
    else
        print ["3 :", "no", "condition"];
    end
    //go to else if
    if false then
        print 1, "first condition";
    else if true then
        print "2 : first condition";
    else
        print ["3 :", "no", "condition"];
    end
    //go to else
    if false then
        print 1, "first condition";
    else if false then
        print "2 : first condition";
    else
        print ["3 :", "no", "condition"];
    end
    print "";

    // simple loop
    while true loop print "loop 1"; break; print "loop 2"; end
    // infinite loop
    //while true loop print "loop 1"; continue; print "loop 2"; end

    print "\nassignements and declaration:";
    x is 3;
    print "x is ", x;
    x := x * 2;
    print "x became ", x;
    print "";

    print "return stm (this is the last print)\n";
    return;
    print "you will never see this text";

end
