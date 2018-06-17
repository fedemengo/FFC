main is func() do

    v is [3, 2];
    print v[0], v[1];

    print "";
    w is [[1, 2, 3], [4, 5, 6]];
    print w[1][1+1];

    print "";
    v[v[0] - 3] := v[0] - 3;
    print v[0];

end