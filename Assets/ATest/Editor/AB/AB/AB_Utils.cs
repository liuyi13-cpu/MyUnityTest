using System;
using System.IO;

namespace com.fanxing.AB
{
    internal class AB_Utils
    {
        /// <summary>
        /// 相对于Assets的目录
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        static internal string GetAssetRelativePath(string fullPath)
        {
            fullPath = fullPath.Replace("\\", "/");
            int index = fullPath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);
            if (index > 0)
            {
                fullPath = fullPath.Substring(index);
            }
            return fullPath;
        }

        /// <summary>
        /// 去后缀名
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        static internal string GetRemoveExtension(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        /// <summary>
        /// 无效资源
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        static internal bool InvalidFile(string fullPath)
        {
            foreach (var item in AB_Const.INVALID_RESOURCE)
            {
                if (fullPath.EndsWith(item)) return true;
            }
            return false;
        }
    }
}
