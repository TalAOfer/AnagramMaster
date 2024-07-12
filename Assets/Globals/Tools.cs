using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    private static readonly Dictionary<float, WaitForSecondsRealtime> WaitRealtimeDictionary = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;
        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    public static WaitForSecondsRealtime GetWaitRealtime(float time)
    {
        if (WaitRealtimeDictionary.TryGetValue(time, out var wait)) return wait;
        WaitRealtimeDictionary[time] = new WaitForSecondsRealtime(time);
        return WaitRealtimeDictionary[time];
    }
}