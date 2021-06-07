using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumerableExtension
{
    public static IEnumerable<T> Yield<T>(this T item)
    {
        yield return item;
    }
}

