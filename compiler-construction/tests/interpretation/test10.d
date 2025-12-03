var mimicAlanGrant := func(value, value2) is
    print "I hate computers!"
    print value, value2
    var array := [value, value2, "I have printed the array!"]
    var tuple := {first := value, second := value2}
    var result := [array[1], tuple.2]
    return result
end

var res
res := mimicAlanGrant("Ah-ha!", "Well...")
print res