# Compute the running median of a sequence of numbers. That is, given a stream of numbers, print out the median of the list so far on each new element.

# Recall that the median of an even-numbered list is the average of the two middle numbers.

# For example, given the sequence [2, 1, 5, 7, 2, 0, 5], your algorithm should print out:


import heapq

def running_median(sequence):
    min_heap = []
    max_heap = []

    for num in sequence:
        # Add the number to the appropriate heap
        if not max_heap or num > -max_heap[0]:
            heapq.heappush(min_heap, num)
        else:
            heapq.heappush(max_heap, -num)

        # Balance the heaps to ensure the size difference is at most 1
        if len(min_heap) - len(max_heap) > 1:
            heapq.heappush(max_heap, -heapq.heappop(min_heap))
        elif len(max_heap) - len(min_heap) > 1:
            heapq.heappush(min_heap, -heapq.heappop(max_heap))

        # Calculate and print the median
        if len(min_heap) == len(max_heap):
            median = (min_heap[0] - max_heap[0]) / 2.0
        elif len(min_heap) > len(max_heap):
            median = min_heap[0]
        else:
            median = -max_heap[0]

        print(median)

# Example usage:
sequence = input()
running_median(sequence)
