var x := 0
loop
    print "Life always finds a way";
    x := x + 1;
    if x > 2 then
        exit
    else
        print "Not yet!"
    end
end

for i in 1..3
    for j in 1..3
        print i, j
    end
end