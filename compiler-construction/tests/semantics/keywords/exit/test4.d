var x := 0
while x < 3 loop
        print "x is small!"
        x := x + 1
        if x = 2 then 
            print "x is not so small :("
            exit
        end
    end