main is func() do
	a is [1, 2, 3, 4, 5];
	
	//x wont modify the array so to read use another approach
	for x in a loop
		x := 3;
		print x;
	end

	print a;
end