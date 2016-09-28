namespace com.fanxing.AB
{
    internal class AB_Const
    {
        /// <summary>
        /// AssetBundle 文件打包类型
        /// </summary>
        internal enum AssetBundleFileType
        {
            /// <summary>
            /// 单文件模式(一个文件一个打包)
            /// </summary>
            AB_SingleFile,

            /// <summary>
            /// 目录文件模式(一个目录所有文件一个打包)
            /// @开头
            /// </summary>
            AB_DirFiles
        }

        internal const string PREFIX_DIR = "@";
        internal const string ASSETS_RESOURCE = "Assets/Resources/Test/";
        internal const string ASSETS_RESOURCE_FILE = "assets/resources/AssetBundle.txt";

        internal static readonly string[] INVALID_RESOURCE =
        {
            ".meta", 
        };
    }
}
