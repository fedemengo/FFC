fatt is func(n : integer) => (if n = 0 then 1 else fatt(n-1) * n end)
fib is func(n : integer) => (if n <= 1 then 1 else fib(n-1) + fib(n-2) end)

print_int is func(n : integer) do
    print n;
    return;
end

main is func() do
    x : integer is read;

    fun is fatt;
    print fun(x);
    
    fun := func(n : integer) => (if n <= 1 then 1 else fib(n-1) + fib(n-2) end);
    print_int(fun(x));
    return;
end

/*

f is func(n : integer) => (if n = 0 then 1 else f(n-1) end)

main is func() do
    print (func()=>(3))();
    return 1;
end


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

n :integer is 3;
g is func() do
    return n;
end

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

fib is func(n: integer) do
    if n <= 1 then
        return 1;
    else
        return fib(n-1) + fib(n-2);
    end
end

z is 5;
bigF is func(a : integer) do
    return func(x : integer) => (z + x + a);
end

k is bigF(3)(5);

*/