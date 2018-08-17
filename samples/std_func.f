a is [1, 2, 3, 4];

aReal is func() => (3.5)

main is func() do
    print length(a);

    x is 1;
    b is 1.4;
    c is 1.5;
    d is 1\3;   // 0.3333
    e is 2\3;   // 0.6666

    print x + round(b);     // 1 + 1
    print x + round(b + c);     // 1 + 3
    print x + round(d);     // 1 + 0
    print x + round(e);     // 1 + 1

    print rat(1);
    print rat(0);
    print rat(3);
    print rat(3, 1);
    print rat(6, 2);
    z is rat(3, 2) + rat(1, 3);
    print z + rat(2, 9);
    print rat(x, 1);

    print compl(1);
    print compl(2.0);
    print compl(2, 3.2);
    print compl(3.1, 4.3);
    print compl(x, b);
    print compl(round(aReal()));

end