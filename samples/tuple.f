main is func() do
    fs is (echo is func(s: string) => (s), sum is func(n: integer) => (n + n));
    print fs.echo("what");
    print fs.sum(3);
    t is (x is 3.1, 2.2);
    print t.1;
    c is (x is 3.1, q is (4.1, x is 9.2));
    print c.x;
    print c.1;
    print c.2.2;
    print c.q.x;
    a is c.q.x;
    print a;
    print c;
    //print a.1.1;
    //print c.q.x.1.1;
    //print c.q.x;
    //b: integer is t.x;
    //print b;
end
