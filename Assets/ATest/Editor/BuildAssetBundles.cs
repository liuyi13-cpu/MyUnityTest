using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        // 命令行参数
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            Debugger.Log(args[i]);
        }
        // BuildPipeline.BuildAssetBundles("Assets/ATest/AssetBundles");
    }

    [MenuItem("Assets/Build AssetBundles clean")]
    static void BuildAllAssetBundlesClean()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets",
            BuildAssetBundleOptions.ForceRebuildAssetBundle
            | BuildAssetBundleOptions.AppendHashToAssetBundleName
            | BuildAssetBundleOptions.UncompressedAssetBundle);
    }
}
