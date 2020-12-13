using System;
using System.Collections.Generic;

public static class Util
{
    public static bool DoWeHaveBadLuck(float probability)
    {
        float rnd = UnityEngine.Random.Range(0f, 1f);
        return rnd < probability;
    }

    /// <summary>Returns a random element picked from a list.</summary>
    public static T PickRandomElement<T>(IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>Returns a random element picked from an array.</summary>
    public static T PickRandomElement<T>(T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}