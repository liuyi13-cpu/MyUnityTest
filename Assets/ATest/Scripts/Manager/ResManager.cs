// #define DEVELOPMENT_BUILD

using AssetBundles;
using System.Collections;
using UnityEngine;

/// <summary>
/// TODO 添加Assetbundle支持
/// </summary>
public class ResManager : MonoBehaviour 
{
    enum ResType
    {
        /// <summary>
        /// 
        /// </summary>
        Resource,
        /// <summary>
        /// 
        /// </summary>
        AssetBundle,
    }

    ResManager(ResType type)
    {
        _CurType = type;
    }

    ResType _CurType = ResType.Resource;

    IEnumerator Start()
    {
        if (_CurType == ResType.AssetBundle)
        {
            yield return _Initialize();
        }
    }

    void _InitializeSourceURL()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        // 测试方式
        AssetBundleManager.SetDevelopmentAssetBundleServer();
#else
        // 本地AB
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
        // 网络AB
        // Or customize the URL based on your deployment or configuration
        //AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
#endif
    }

    protected IEnumerator _Initialize()
    {
        DontDestroyOnLoad(gameObject);

        _InitializeSourceURL();

        var request = AssetBundleManager.Initialize();
        if (request != null)
            yield return StartCoroutine(request);
    }

    public GameObject Load(string path)
    {
        if (_CurType == ResType.Resource)
        {
            return Resources.Load(path) as GameObject;
        }
        else
        {
            GameObject go = null;
            // go = AssetBundleManager.LoadAsset();
            return go;
        }
    }

    public GameObject Instantiate(string path)
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
