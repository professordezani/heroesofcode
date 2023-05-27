# -*- coding: utf-8 -*-

# Dada uma lista de números e um número k, retorne se existe algum par de números na lista cuja soma seja igual a k.

# For example, given [10, 15, 3, 7] and k of 17, return true since 10 + 7 is 17.
# Do this in one pass using python code.

def check_sum(nums, k):
    complement_set = set()

    for num in nums:
        complement = k - num
        if complement in complement_set:
            return True
        complement_set.add(num)

    return False

nums = input()
k = input()

print(check_sum(nums, k))