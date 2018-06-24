main is func(): integer do
    e is [func() do print "hello"; end];
    a is 3;
    b is [4.4, 1.0, 9.99, 100.01, 0.1];
    c is 4\4;
    d is 4.0i5.9;
    e is [1, 2, 3];
    f is e + -a;
    print a, b, c, d, e;
    print f;
    f := [1];
    print f;
end
