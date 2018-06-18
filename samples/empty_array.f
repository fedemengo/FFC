main is func() do

    a :[integer] is [];
    b is a + 3;
    c is [1, 2, 3] + a;
    print a, b, c;
    a := a + b + c;
    print a;

    a := [];
    print a;

end