var x := 0
while x < 5 loop
        print "x is small!"
        x := x + 1
        if x = 3 then 
            print "x is not so small anymore :("
            exit
        end
    end
    
var a := ["awesome", "pretty", "really", "are", "dinosaurs"]
var n := 5
var e := []
for i in 1..n+1 loop
    e[n-i+1] := a[i]
end

print e