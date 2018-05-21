min is func(x1: integer, x2: integer) => if x1 < x2 then x1 else x2 end

max is func(x1: integer, x2: integer) => if x1 > x2 then x1 else x2 end

mot is func(x1: integer, x2: integer, x3: integer) => max(min(x1, x2), min(max(x1, x2), x3)) end

pivot is func(v: [integer]) do
    m is round((length(v) - 1) / 2);
    return mot(v[0], v[m], v[length(v)-1]);
end

partitions is func(v: [integer]) do
    p is pivot(v);
    v1 is [integer];
    v2 is [integer];
    for i in v loop
        if i <= p then
            v1 := v1 + i;
        else
            v2 := v2 + i
        end
    end
    return (v1, v2);
end

quicksort is func(v: [integer]) do
    if length(v) <= 1 then
        return v;
    end
    parts is partitions(v);
    parts := (quicksort(parts.1), quicksort(parts.2));
    return parts.1 + parts.2
end
