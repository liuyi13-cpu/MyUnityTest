
using System;
using UnityEngine;

public static class Utils
{
    public static void ClearMemory()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}