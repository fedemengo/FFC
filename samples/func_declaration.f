f is func(n: integer) do
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