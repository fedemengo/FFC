main is func() do
    print true = false, false = false, false /= false, false /= true;
    print 1.0i0.0 = 1i0, 1.0i0.0 /= 1i0.1, 1.0i0.0 <= 1i0.1;
    print 1\3 = 1\3, 5\4 /= 5\4, 11\54 <= 12\54;
    print "h" = "H", "h" /= "H", "h" = "h";
end