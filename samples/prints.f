//currently, assignements are not runnable because\
    they leave stuff in the stack which must be empty\
    at the end of each 'il function'

main is func() do
    //basic int
    print 3;
    //int expressions
    print 3 * 2;
    //int arrays
    print [1];
    print [3, 4];
    //any type arrays
    print [2.4];
    print [[1, 2], [3, 4]];
    //arrays concatenation
    print [3] + [3];
    print [[1, 2], [3, 4]] + [[5, 6], [7, 8]];
    //arrays additions : to do
    //print [3] + 4;
end