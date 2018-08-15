a is [1, 2, 3, 4];

main is func() do
    print length(a);

    x is 1;
    b is 1.4;
    c is 1.5;
    d is 1\3;   // 0.3333
    e is 2\3;   // 0.6666

    print x + round(b);     // 1 + 1
    print x + round(c);     // 1 + 2
    print x + round(d);     // 1 + 0
    print x + round(e);     // 1 + 1    
end