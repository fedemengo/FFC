binsearch is func(v: [integer], k: integer) do
	l is 0;
	r is length(v) - 1;
	while l < r loop
		m is round((l + r) / 2);
		if v[m] = k then
			return m;
		else if v[m] < k then
			r := m - 1;
		else
			l := m + 1;
		end end
	end
	return -1;
end;
