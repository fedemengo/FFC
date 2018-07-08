main is func() do
    a : integer is [1, 2, 3];
    b is [0];
    c is a + b + a + b + b;
    c := c + a;
    print a, b, c;
    a := b;

end