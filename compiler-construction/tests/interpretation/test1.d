print "HelloWorld!"
var b := 10
var tuple := {a := 3, b};
print "initial value for tuple element:", tuple.a;
tuple.a := 4;
print "new value for tuple element:", tuple.a;
tuple.2 := "new_value_for_b";
print tuple.2
var array := [2, "stringsome", 2.3]
print "initial value for array element:", array[1];
array[1] := 5;
print "new value for array element:", array[1];