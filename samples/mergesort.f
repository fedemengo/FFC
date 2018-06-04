merge is func(a: [integer], b: [integer]) do
	v: [integer] is [];
	i is 0;
	j is 0;
	while i < length(a) & j < length(b) loop
		if a[i] < b[j] then
			v := v + a[i];
			i := i + 1;
		else
			v := v + b[j];
			j := j + 1;
		end
	end
	while i < length(a) loop
		v := v + a[i];
		i := i + 1;
	end
	while j < length(b) loop
		v := v + b[j];
		j := j + 1;
	end
end

// commento

split is func(v: [integer], pos: integer) do
	i is 0;
	a: [integer] is [];
	b: [integer] is [];
	while i < length(v) loop
		if i < pos then
			a := a + v[i];
		else
			b := b + v[i];
		end
		i := i + 1;
	end
	return (a, b);
end


mergesort is func(v: [integer]) do
	parts is split(v, round(length(v)/2));
	return merge(parts.1, parts.2);
end

main is func() do
	v is [3, 5, 1, 2, 4, -2, 8];
	s is mergesort(v);
	for i in s loop print(i); end
end

