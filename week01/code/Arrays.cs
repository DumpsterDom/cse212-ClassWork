using System.Collections.Generic; 

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size length starting with number followed by multiples of number.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.

    public static double[] MultiplesOf(double number, int length)
    {
        // Step-by-step plan:
        // 1. Create a new array of doubles with exactly length slots.
        // 2. Start with the first multiple, which is simply number.
        // 3. Each subsequent multiple is the previous one plus number.
        // 4. Loop length times:
        //    - Calculate the current multiple: number * (i + 1)
        //    - Or, keep a running value: current = current + number
        // 5. Store each value in the array at index i.
        // 6. Return the completed array.

        double[] result = new double[length];

        // Using direct multiplication
        for (int i = 0; i < length; i++)
        {
            result[i] = number * (i + 1);
        }

        return result;
    }

    /// <summary>
    /// Rotate the data to the right by the amount.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // Step-by-step plan:
        // 1. If the list is empty or has only one element, nothing to do.
        // 2. Because amount can be any value from 1 to data.Count,
        //    we can normalize it with modulo to avoid unnecessary full rotations:
        // 3. A clean way to rotate right is:
        //    - Take the last effectiveAmount elements - they go to the front.
        //    - Take the first (data.Count - effectiveAmount) elements - they go after.
        // 4. We can use List<int> methods:
        //    - GetRange to extract slices
        //    - RemoveRange + InsertRange or AddRange to rebuild the list
        // 5. Alternative (more concise): create a new list with the two slices
        //    and then copy back into the original list.

        if (data == null || data.Count <= 1)
            return;

        int n = data.Count;
        int effectiveAmount = amount % n;

        if (effectiveAmount == 0)
            return; 

        // Approach using GetRange
        List<int> lastPart = data.GetRange(n - effectiveAmount, effectiveAmount);        
        List<int> firstPart = data.GetRange(0, n - effectiveAmount);                    

        // Rebuild the list: lastPart first, then firstPart
        data.Clear();
        data.AddRange(lastPart);
        data.AddRange(firstPart);
    }
}