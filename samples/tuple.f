main is func() do
    fs is (echo is func(s: string) => (s), sum is func(n: integer) => (n + n));
    print fs.echo("what");
    print fs.sum(3);
    t is (x is 12.14, y is 2.2);
    print t.1;
    print t.x;
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
end
