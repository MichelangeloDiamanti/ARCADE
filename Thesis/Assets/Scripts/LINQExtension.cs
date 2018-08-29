using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pddl = ru.cadia.pddlFramework;

public static class LINQExtension
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        IEnumerable<IEnumerable<T>> result = emptyProduct;
        foreach (IEnumerable<T> sequence in sequences)
        {
            result = from accseq in result from item in sequence select accseq.Concat(new[] { item });
        }
        return result;
    }

    public static List<List<T>> CartesianProduct<T>(this List<List<T>> sequences)
    {
        List<List<T>> result = new List<List<T>>();

        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        IEnumerable<IEnumerable<T>> enumResult = emptyProduct;
        foreach (IEnumerable<T> sequence in sequences)
        {
            enumResult = from accseq in enumResult from item in sequence select accseq.Concat(new[] { item });
        }

        foreach (IEnumerable<T> seq in enumResult)
            result.Add(seq.ToList());

        return result;
    }

    public static IEnumerable<string> shortToString(this IEnumerable<pddl.Action> actions)
    {
        List<string> strings = new List<string>();
        foreach (pddl.Action a in actions)
        {
            strings.Add(a.shortToString());
        }

        return strings;
    }

    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> sequence)
    {
        if (sequence == null)
        {
            yield break;
        }

        var list = sequence.ToList();

        if (!list.Any())
        {
            yield return Enumerable.Empty<T>();
        }
        else
        {
            var startingElementIndex = 0;

            foreach (var startingElement in list)
            {
                var remainingItems = list.AllExcept(startingElementIndex);

                foreach (var permutationOfRemainder in remainingItems.Permute())
                {
                    yield return startingElement.Concat(permutationOfRemainder);
                }

                startingElementIndex++;
            }
        }
    }

    private static IEnumerable<T> Concat<T>(this T firstElement, IEnumerable<T> secondSequence)
    {
        yield return firstElement;
        if (secondSequence == null)
        {
            yield break;
        }

        foreach (var item in secondSequence)
        {
            yield return item;
        }
    }

    private static IEnumerable<T> AllExcept<T>(this IEnumerable<T> sequence, int indexToSkip)
    {
        if (sequence == null)
        {
            yield break;
        }

        var index = 0;

        foreach (var item in sequence.Where(item => index++ != indexToSkip))
        {
            yield return item;
        }
    }

    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
