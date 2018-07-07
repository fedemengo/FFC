main is func() do
    // test complex
    a is 3i4;
    b is 3.1i4;
    c is 3i4.1;
    d is 3.1i4.1;
    a1 is -3i-4;
    b1 is -3.1i-4;
    c1 is -3i-4.1;
    d1 is -3.1i-4.1;
    print a, b, c, d;
    print a1 * 2, b1 * 2, c1 * 2, d1 * 2;
    print a * b, c * d;
    print a * b / c * d;
    
    i: integer is 1;
    d: real is -3.57;
    r: rational is -7\3;
    c: complex is -5.3i0.0;
    s: string is "Hello world!\n";
    a: string is "I don't like \"Hello world!\\n\" message\n\tBut I guess I'll have to live with them.\a";
    //z: [{(integer, integer) : (integer, integer)}] is [{(0, 10) : (0, -10), (10, 20) : (-10, -20)}, {(100, 1000) : (300, 3000)}];

    //e is ([1, 2, 3], {["a", "b"] : (1, 3\4, "hi")}, 1\4, 3.1i4.5);
    return 1;
end