a is func(n : integer) do
    if n > 0 then
        return a(n-1);
    else
        print n;
    end
end

f is func(n : integer) do
    if n > 0 then
        a is f(n-1);
        return f(n-1) * n;
    end
    return 1;
end
        

main is func() do
	//this throws an exception
    // x is a(5);
    //this still works ok
    print f(5);
	return;
end