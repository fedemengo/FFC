main is func() do
    
    // + - *

    print 1 + 1;
    print 1 - 1.5;
    print 1.5 - 1;
    print 1.5 * 1.5;

    print 1 + 1\2;
    print 1\2 + 1;
    print 1\2 + 1\2;
    
    print 1 + 1.0i1.0;
    print 1.5 + 1.0i1.0;
    print 1.0i1.0 - 1;
    print 1.0i1.0 - 1.5;
    print 1.0i1.0 * 2.0i-3.5;

    // /

    print 1 / 2;
    print 1 / 2.5;
    print 2.5 / 2;
    print 2.5 / 1.5;

    print 1 / 2\1;
    print 2\1 / 3;
    print 3\4 / 4/7;

    print 1.0i1.0 / 3;
    print 1.0i1.0 / 1.5;
    print 1.0i1.0 / 2.0i-2.0;

    // shall we really not support (integer or real) / complex ?

    print 1 / 1.0i1.0;
    print 1.5 / 1.0i1.0;

    // = /= < <= > >=

    print 1 < 1.5; 
    print 1 >= 4\3;
    print 1 = 1.0; 
    print 5.0 /= 7; 
    
    //can't be done, good ! print 1.3i0.0 < 2;

end