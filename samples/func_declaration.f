a is func() : integer do
    print 3;
    return 1;
end

/*

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