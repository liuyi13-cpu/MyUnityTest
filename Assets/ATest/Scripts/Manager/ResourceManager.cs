using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : Base
{
    const string PATH = "D:/work/github/MyUnityTest/AssetBundles/Android/";
    const string MANIFEST_NAME = "Android";

    Dictionary<string, AssetBundle> cacheAB = new Dictionary<string, AssetBundle>();

    public override void StartEx()
    {

    }

    public static T Load<T>(string path) where T : Object
    {
        return AssetBundle.LoadFromFile(path) as T;
#if UNITY_EDITOR
        return Resources.Load<T>(path);
#else

#endif
    }

    public void loadAssetBundleManifest()
    {
        var ab = loadAssetBundles(MANIFEST_NAME, false);
        if (ab)
        {
            var obj = ab.LoadAsset("AssetBundleManifest");
            var manifest = obj as AssetBundleManifest;
            if (manifest)
            {
                var arr = manifest.GetAllAssetBundles();
                for (int i = 0; i < arr.Length; i++)
                {
                    Debugger.LogWarning(arr[i]);

                    var tmp = manifest.GetAllDependencies(arr[i]);
                    for (int j = 0; j < tmp.Length; j++)
                    {
                        Debugger.LogWarning("----" + tmp[j]);
                        loadAssetBundles(tmp[j]);
                    }

                    loadAssetBundles(arr[i]);
                }
            }
            // unload
            ab.Unload(true);
        }
    }

    AssetBundle loadAssetBundles(string fileName, bool needCache = true)
    {
        if (cacheAB.ContainsKey(fileName))
        {
            return cacheAB[fileName];
        }

        var fullPathName = PATH + fileName;
        if (!File.Exists(fullPathName))
        {
            Debugger.LogError(fullPathName);
            return null;
        }

        Debugger.Log("load ab " + fileName);

        var ab = AssetBundle.LoadFromFile(fullPathName);
        if (needCache)
        {
            cacheAB[fileName] = ab;
        }

        var arr = ab.GetAllAssetNames();
        for (int i = 0; i < arr.Length; i++)
        {
            var path = arr[i];
            Debugger.Log(path);

            if (path.LastIndexOf('/') != -1)
            {
                path = path.Substring(path.LastIndexOf('/') + 1);
            }
            var asset = ab.LoadAsset(path);
            if (asset)
            {
                Debugger.Log("load asset " + asset.ToString());
            }
        }
        return ab;
    }

    public void Dispose()
    {
        cacheAB.Clear();
    }
}
