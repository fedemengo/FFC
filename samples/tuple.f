main is func() do
    fs is (echo is func(s: string) => (s), sum is func(n: integer) => (n + n));
    print fs.echo("what");
    print fs.sum(3);
    t is (x is 12.14, y is 2.2);
    c is (x is 3.1, q is (4.1, x is 9.2));
    print c.x;
    print c.1;
    print c.2.2;
    print c.q.x;
    a is c.q.x;
    print a;
    print c;

    d is (r is 0.01, c is 3.2);
    print d;
    d := t;
    print d.1;
    //print d.x; compilation error
    print d.r;

    /*
        TEST
    */
    t1 is (true, (1\1, 1), -1, ("-1", 1), -1.1);
    print t1;

    v1 is [t1];
    //v1 := v1 + t1;  error
    v1 := v1 + (false, (1231\171442, 1942), 831, ("world!", 1), 231.112);  
    print v1;

    t2 is (f is func(x: integer) do 
                y is 0;
                for i in 0..x loop 
                    y := y + i; 
                end
                return y; 
            end, 
            func() do 
                return "Some text!"; 
            end);

    print t2.f(5);
    print t2.2();

    t2.f := func(x: integer) do
                return x * 42;
            end;

    print t2.f(5);

    t1.1 := false;
end
