main is func() do
    n : integer is read;
    v : [integer] is [];
    
    //should capture v
    add is func(n : integer) do
        v := v + n;
    end

    for i in 1..n loop
        a : integer is read;
        add(a);
    end

    less is func(a : integer, b : integer) => (a < b)
    
    swap is func(a : integer, b : integer) do
        c is v[a];
        v[a] := v[b];
        v[b] := c;
    end

    //iterative in-place insertion sort, should capture swap only
    sort is func(v : [integer], from : integer, to : integer, cmp : func(integer, integer) : boolean) do
        for i in 1..to loop
            print "currently at", i;
            while i > 0 & cmp(v[i], v[i-1]) loop
                print "swappin'", i, i-1;
                swap(i, i-1);
                print "swapped";
                i := i-1;
            end
        end
    end

    sort(v, 0, n-1, less);
    print v;

end