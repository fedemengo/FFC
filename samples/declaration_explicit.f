main is func(): integer do
    a:integer is 3;
    b: [real] is [4.4, 1.0, 9.99, 100.01, 0.1];
    c: rational is 4\4;
    d: complex is 4.0i5.9;
    e: [integer] is [1, 2, 3];
    f: [integer] is e + -a;
    g: string is "hello";
    h: [func():integer] is [func() => (1)];
    l: [func(integer):integer] is [func(x: integer) => (x)];
    // how to specify void return type
    //e: [func(string):integer] is [func(s: string) do print s; end];
end
