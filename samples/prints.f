//currently, assignements are not runnable because\
    they leave stuff in the stack which must be empty\
    at the end of each 'il function'

main is func() do
    //basic int
    print 3;
    
    //int expressions
    print 3 * 2;
    
    //other types
    print 1.0i-2.0;
    
    //int arrays
    print [1];
    print [3, 4];
    
    //any type arrays
    print [2.4];
    print [[1, 2], [3, 4]];
    
    //arrays concatenation
    print [3] + [3];
    print [[1, 2], [3, 4]] + [[5, 6], [7, 8]];
    print [[[1]]] + [[[1]]] + [[[1, 2],[3],[4, 5, 6]]];

    //arrays array additions
    print [3] + 4;
    print [1,3] + ([1, 2] + 4);
    print [1.3] + 2.4 + 2.3;
    print [[1]] + [1];
    print [[1]] + [0] + [[1], [0]] + [1];

end