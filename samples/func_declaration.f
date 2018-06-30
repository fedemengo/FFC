fatt is func(n : integer) => (if n = 0 then 1 else fatt(n-1) * n end)
fib is func(n : integer) => (if n < 1 then 1 else fib(n-1) + fib(n-2) end)

print_int is func(n : integer) do
    print n;
end

bad_func is func(a : integer) do
    print a;
    if a > 0 then return bad_func(a - 2); end
end

cool is func() do
    print (func()=>(3))();
    return 1;
end


f is func(n : integer) => (if n = 0 then 1 else f(n-1) end)


a is func() do
    
    x: integer is read;
    
    square is func(n: integer) do 
        return n * n; 
    end
    
    print square(x);
    
    z: integer is square(x);
    print z + 1;

    return 0;
end

easiest is func() : integer do
    return 1;
end

arrowStyle is func() => (easiest)

b is func() do
    print a();
end

square is func(n : integer) do
    return n * n;
end

f is func(n: integer) : integer do
    if n = 0 then
        return 1;
    else
        return n * f(n - 1);
    end
end

fib2 is func(n: integer) do
    if n <= 1 then
        return 1;
    else
        return fib(n-1) + fib(n-2);
    end
end

main is func() do
    x : integer is read;

    bad_func(x);

    fun is fatt;
    print fun(x);

    fun := func(n : integer) => (if n <= 1 then 1 else fib(n-1) + fib(n-2) end);

    print fib(x);
    
    print_int(fun(x));

    array is [func(n : integer) => (n * n), func(n : integer) => (n * n * n)];
    print array[0](x), array[1](x);

    cool();

    print arrowStyle()();

    b();

    print f(x);

    print fib2(x);

end

/*

n :integer is 3;
g is func() do
    return n;
end

z is 5;
bigF is func(a : integer) do
    return func(x : integer) => (z + x + a);
end

k is bigF(3)(5);

*/