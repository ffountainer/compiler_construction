var x := 0
while x < 5 loop
        print "x is small!"
        x := x + 1
        if x = 3 then 
            print "x is not so small anymore :("
            exit
        end
    end