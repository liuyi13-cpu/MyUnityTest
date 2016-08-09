using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/ATest/AssetBundles");
    }

    [MenuItem("Assets/Build AssetBundles clean")]
    static void BuildAllAssetBundlesClean()
    {
        BuildPipeline.BuildAssetBundles("Assets/ATest/AssetBundles1",
            BuildAssetBundleOptions.ForceRebuildAssetBundle
            | BuildAssetBundleOptions.AppendHashToAssetBundleName
            | BuildAssetBundleOptions.UncompressedAssetBundle);
    }
}
