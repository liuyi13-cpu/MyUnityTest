using UnityEditor;

namespace com.fanxing.AB
{
    public class AB_Build
    {
        [MenuItem("AssetBundle/打包Android")]
        static void BuildAB()
        {
            // 1.设置文件的AssetBundle name
            // 2.导出自定义的AssetBundleManifest
            // 3.打包

            AB_AssetBundleManifest manifest = new AB_AssetBundleManifest();
            manifest.SetAssetBundleName();
        }

        static void BuildAllAssetBundlesClean()
        {
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets",
                BuildAssetBundleOptions.ForceRebuildAssetBundle
                | BuildAssetBundleOptions.AppendHashToAssetBundleName
                | BuildAssetBundleOptions.UncompressedAssetBundle);
        }

        [MenuItem("AssetBundle/清理assetBundleName")]
        static void ClearAllAssetNames()
        {
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            if (assetBundleNames == null || assetBundleNames.Length <= 0)
                return;
            for (int i = 0; i < assetBundleNames.Length; ++i)
            {
                float process = ((float)i) / ((float)assetBundleNames.Length);
                EditorUtility.DisplayProgressBar("清理assetBundleName", assetBundleNames[i], process);
                AssetDatabase.RemoveAssetBundleName(assetBundleNames[i], true);
            }
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            EditorUtility.UnloadUnusedAssetsImmediate();
        }
    }
}