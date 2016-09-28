using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace com.fanxing.AB
{
    // 自定义Manifest文件
    internal class AB_AssetBundleManifest
    {
        /// <summary>
        /// <Asset, AssetBundle>
        /// </summary>
        Dictionary<string, string> _mapAB = new Dictionary<string, string>();

        internal void SetAssetBundleName()
        {
            string fullRootPath = Path.GetFullPath(AB_Const.ASSETS_RESOURCE);
            string[] dirs = Directory.GetDirectories(fullRootPath, "*", SearchOption.AllDirectories);

            int startIndex = 0;
            float total = dirs.Length;
            foreach (var item in dirs)
            {
                startIndex++;
                bool isCancel = EditorUtility.DisplayCancelableProgressBar("SetAssetBundleName", item, startIndex / total);
                if (isCancel)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }

                Debugger.Log(item);
                if (item.Contains(AB_Const.PREFIX_DIR))
                {
                    // 按文件夹打包
                    _SetAssetBundleName(item, false);
                }
                else
                {
                    // 单文件打包
                    _SetAssetBundleName(item, true);
                }
            }

            _CreateFile();

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            EditorUtility.UnloadUnusedAssetsImmediate();
        }

        void _SetAssetBundleName(string dir, bool separate)
        {
            string[] files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
            foreach (var item in files)
            {
                if (AB_Utils.InvalidFile(item))
                {
                    // 无效资源
                    continue;
                }

                string assetBundleName;
                if (separate)
                {
                    assetBundleName = AB_Utils.GetRemoveExtension(item);
                }
                else
                {
                    assetBundleName = Path.GetDirectoryName(item);
                }
                assetBundleName = AB_Utils.GetAssetRelativePath(assetBundleName);
                assetBundleName = assetBundleName.Replace('/', '$');

                string subFileShortName = AB_Utils.GetAssetRelativePath(item);

                // 1.设置assetBundleName
                _SetAssetBundleName(subFileShortName, assetBundleName);

                // 2.添加映射
                _mapAB.Add(subFileShortName, assetBundleName);
            }
        }

        /// <summary>
        /// 设置assetBundleName
        /// </summary>
        /// <param name="item"></param>
        /// <param name="assetBundleName"></param>
        void _SetAssetBundleName(string item, string assetBundleName)
        {
            AssetImporter importer = AssetImporter.GetAtPath(item);
            if (importer != null
                && !importer.assetBundleName.Equals(assetBundleName))
            {
                importer.assetBundleName = assetBundleName;
            }
        }

        /// <summary>
        /// 导出_mapAB到文件
        /// </summary>
        void _CreateFile()
        {
            // 1.创建txt
            string fullRootPath = Path.GetFullPath(AB_Const.ASSETS_RESOURCE_FILE);
            if (File.Exists(fullRootPath)) File.Delete(fullRootPath);

            using (FileStream fs = new FileStream(fullRootPath, FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var item in _mapAB)
                    {
                        sw.WriteLine(item.Key + "|" + item.Value);
                    }
                    sw.Close();
                }
                fs.Close();
            }

            string assetBundleName = AB_Utils.GetRemoveExtension(AB_Const.ASSETS_RESOURCE_FILE);
            assetBundleName = assetBundleName.Replace("\\", "/").Replace('/', '$');
            // 2.设置txt的assetBundleName
            _SetAssetBundleName(AB_Const.ASSETS_RESOURCE_FILE, assetBundleName);
        }
    }
}