using System;
using System.Collections.Generic;

public static class Util
{
    public static bool DoWeHaveBadLuck(float probability)
    {
        float rnd = UnityEngine.Random.Range(0f, 1f);
        return rnd < probability;
    }
}