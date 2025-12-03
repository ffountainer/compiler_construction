var xs := [1, 2, 3, 4, 5]
var ys := [5, 7, 9, 11, 13]

var n := 0
for i in xs loop
    n := n + 1
end

var sum_x := 0
var sum_y := 0
var sum_xy := 0
var sum_x2 := 0

for i in xs loop
    var x := xs[i]
    var y := ys[i]
    sum_x := sum_x + x
    sum_y := sum_y + y
    sum_xy := sum_xy + x * y
    sum_x2 := sum_x2 + x * x
end

var num := n * sum_xy - sum_x * sum_y
var den := n * sum_x2 - sum_x * sum_x

var a := num / den
var b := (sum_y - a * sum_x) / n

print "slope = ", a, ", intercept = ", b
