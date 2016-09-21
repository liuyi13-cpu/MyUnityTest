using UnityEngine;

/// <summary>
/// TODO 添加Assetbundle支持
/// </summary>
public class ResManager
{
    public static GameObject Load(string path)
    {
        return Resources.Load(path) as GameObject;
    }

    public static GameObject Instantiate(string path)
    {
        GameObject go = Load(path);
        return Object.Instantiate(go);
    }

    public static void Destroy(Object obj)
    {
        // Debugger.Log("ResManager Destroy : " + obj.ToString());
        Object.Destroy(obj);
    }

    public static void DestroyImmediate(Object obj)
    {
        // Debugger.Log("ResManager DestroyImmediate : " + obj.ToString());
        Object.DestroyImmediate(obj);
    }
}
