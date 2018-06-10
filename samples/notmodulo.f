pari is func(a: integer) do
    return a % 2 = 0;
end

dispari is func(a : integer) do
    return ! pari(a);
end