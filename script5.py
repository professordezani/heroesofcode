# A Collatz sequence in mathematics can be defined as follows. Starting with any positive integer:

# if n is even, the next number in the sequence is n / 2
# if n is odd, the next number in the sequence is 3n + 1
# It is conjectured that every such sequence eventually reaches the number 1. Test this conjecture using python code.


def collatz_conjecture(n):
    sequence = [n]

    while n != 1:
        if n % 2 == 0:
            n = n // 2
        else:
            n = 3 * n + 1
        sequence.append(n)

    return sequence

# Test the conjecture for a given number
number = input()

sequence = collatz_conjecture(number)
# print(len(sequence))
print(sequence)
