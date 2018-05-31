using System.Collections;
using System.Collections.Generic;
using System;

public static class Utils
{
    /// <summary>
    /// Method to create lists containing possible combinations of an input list of items. 
    /// </summary>
    /// <typeparam name="T">type of the items on the input list</typeparam>
    /// <param name="inputList">list of items</param>
    /// <param name="minimumItems">minimum number of items wanted in the generated combinations, 
    ///                            if zero the empty combination is included,
    ///                            default is one</param>
    /// <param name="maximumItems">maximum number of items wanted in the generated combinations,
    ///                            default is no maximum limit</param>
    /// <returns>list of lists for possible combinations of the input items</returns>
    public static List<List<T>> ItemCombinations<T>(List<T> inputList, int minimumItems = 1,
                                                    int maximumItems = int.MaxValue)
    {
        int nonEmptyCombinations = (int)Math.Pow(2, inputList.Count) - 1;
        List<List<T>> listOfLists = new List<List<T>>(nonEmptyCombinations + 1);

        // Optimize generation of empty combination, if empty combination is wanted
        if (minimumItems == 0)
            listOfLists.Add(new List<T>());

        if (minimumItems <= 1 && maximumItems >= inputList.Count)
        {
            // Simple case, generate all possible non-empty combinations
            for (int bitPattern = 1; bitPattern <= nonEmptyCombinations; bitPattern++)
                listOfLists.Add(GenerateCombination(inputList, bitPattern));
        }
        else
        {
            // Not-so-simple case, avoid generating the unwanted combinations
            for (int bitPattern = 1; bitPattern <= nonEmptyCombinations; bitPattern++)
            {
                int bitCount = CountBits(bitPattern);
                if (bitCount >= minimumItems && bitCount <= maximumItems)
                    listOfLists.Add(GenerateCombination(inputList, bitPattern));
            }
        }

        return listOfLists;
    }

    /// <summary>
    /// Sub-method of ItemCombinations() method to generate a combination based on a bit pattern.
    /// </summary>
    private static List<T> GenerateCombination<T>(List<T> inputList, int bitPattern)
    {
        List<T> thisCombination = new List<T>(inputList.Count);
        for (int j = 0; j < inputList.Count; j++)
        {
            if ((bitPattern >> j & 1) == 1)
                thisCombination.Add(inputList[j]);
        }
        return thisCombination;
    }

    /// <summary>
    /// Sub-method of ItemCombinations() method to count the bits in a bit pattern. Based on this:
    /// https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetKernighan
    /// </summary>
    private static int CountBits(int bitPattern)
    {
        int numberBits = 0;
        while (bitPattern != 0)
        {
            numberBits++;
            bitPattern &= bitPattern - 1;
        }
        return numberBits;
    }
    
        /// <summary>
        /// Heap's algorithm to find all pmermutations. Non recursive, more efficient.
        /// </summary>
        /// <param name="items">Items to permute in each possible ways</param>
        /// <param name="funcExecuteAndTellIfShouldStop"></param>
        /// <returns>Return true if cancelled</returns> 
        public static bool ForAllPermutation<T>(T[] items, Func<T[], bool> funcExecuteAndTellIfShouldStop)
        {
            int countOfItem = items.Length;

            if (countOfItem <= 1)
            {
                return funcExecuteAndTellIfShouldStop(items);
            }

            var indexes = new int[countOfItem];
            for (int i = 0; i < countOfItem; i++)
            {
                indexes[i] = 0;
            }

            if (funcExecuteAndTellIfShouldStop(items))
            {
                return true;
            }

            for (int i = 1; i < countOfItem;)
            {
                if (indexes[i] < i)
                { // On the web there is an implementation with a multiplication which should be less efficient.
                    if ((i & 1) == 1) // if (i % 2 == 1)  ... more efficient ??? At least the same.
                    {
                        Swap(ref items[i], ref items[indexes[i]]);
                    }
                    else
                    {
                        Swap(ref items[i], ref items[0]);
                    }

                    if (funcExecuteAndTellIfShouldStop(items))
                    {
                        return true;
                    }

                    indexes[i]++;
                    i = 1;
                }
                else
                {
                    indexes[i++] = 0;
                }
            }

            return false;
        }
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

}
