using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CheckFile.dataHelper
{
    [Serializable]
    public class FileData
    {
        //public bool IsReceive { get; set; }

        //public bool IsSend { get; set; }

        public string Name { get; set; }

        public string Root { get; set; }

        public int Length { get; set; }

        public string FullName { get; set; }

        public string ParentName { get; set; }

        public string MD5 { get; set; }

        public string NameOnly { get; set; }

        //public FileData()
        //{
        //    IsSend = IsReceive = false;
        //}
    }

    [Serializable]
    public class FloderData
    {

        public List<FileData> FileDataList { get; set; }

        public string FullPath { get; set; }

        public int Length { get; set; }

        public FloderData()
        {
            FileDataList = new List<FileData>();
        }
    }

    public static class DirectoryHelper
    {

        public static List<FileData> GetAllFiles(string path)
        {
            var DirectoryInfo = new System.IO.DirectoryInfo(path);
            return DirectoryHelper.GetAllFiles("", DirectoryInfo.Name, DirectoryInfo.Parent.FullName).ToList();
        }

        public static IEnumerable<FileData> GetAllFiles(string path, string parentname = "", string rootname = "")
        {
            var realpath = Path.Combine(rootname, parentname, path);
            DirectoryInfo di = new DirectoryInfo(realpath);
            if (di.Exists)
            {
                foreach (var item in di.GetFiles())
                {
                    var p = Path.Combine(path, parentname);
                    var f = GetFileDataInfo(item.FullName, path, parentname, rootname);
                    if (f != null)
                    {
                        yield return f;
                    }
                }
                foreach (var item in di.GetDirectories())
                {
                    if ((item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        var pn = string.IsNullOrWhiteSpace(path) ? item.Name : path + "\\" + item.Name;
                        var fl = GetAllFiles(pn, parentname, rootname).ToList();
                        foreach (var filedatainfo in fl)
                        {
                            yield return filedatainfo;
                        }
                    }
                }
            }
        }

        public static FileData GetFileDataInfo(string filepath, string path, string parentname, string root)
        {
            FileInfo fileinfo = new FileInfo(filepath);
            if (checkIg(filepath))
            {
                if (fileinfo.Exists && (fileinfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    var fd = new FileData();
                    fd.MD5 = MD5Helper.GetFastMD5(fileinfo.FullName);
                    fd.Length = (int)fileinfo.Length;
                    fd.FullName = fileinfo.FullName;
                    fd.NameOnly = fileinfo.Name.ReplaceLast(fileinfo.Extension, "");
                    fd.ParentName = parentname;
                    fd.Root = root;
                    fd.Name = string.IsNullOrWhiteSpace(path) ? fileinfo.Name : path + "\\" + fileinfo.Name;
                    return fd;
                }
            }
            return null;
        }

        public static string PathCombin(this DirectoryInfo directory, string path)
        {
            DirectoryInfo realdir = new DirectoryInfo(directory.FullName);
            var directroysplit = path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            int dircount = directroysplit.Count() - 1;
            if (dircount >= 0)
            {
                var name = directroysplit[dircount];
                for (int i = dircount; i > 0; i--)
                {
                    if (directroysplit[i] == "..")
                    {
                        realdir = realdir.Parent;
                    }
                    else if (i != dircount && directroysplit[i] != ".")
                    {
                        name = directroysplit[i] + "\\" + name;
                    }
                }
                return realdir.FullName + "\\" + name;
            }
            return null;
        }

        public static string PathCombinSimple(this DirectoryInfo directory, string path)
        {
            var fileinfo = new FileInfo(path);

            List<DirectoryInfo> firstdirs = new List<DirectoryInfo>();

            while (true)
            {
                firstdirs.Add(directory);
                if (directory.FullName.Equals(directory.Root.FullName))
                {
                    break;
                }
                directory = directory.Parent;
            }

            var filedir = fileinfo.Directory;
            int count = firstdirs.Count;
            string filenamedir = "";
            while (true)
            {
                for (int i = 0; i < count; i++)
                {
                    if (filedir.FullName.Equals(firstdirs[i].FullName))
                    {
                        var realpath = "";
                        for (int j = i; j > 0; j--)
                        {
                            realpath += "..\\";
                        }
                        return realpath + filenamedir + "\\" + fileinfo.Name;
                    }
                }

                if (filedir.Parent == null)
                {
                    return null;
                }
                filenamedir = !string.IsNullOrWhiteSpace(filenamedir) ? (filedir.Name + "\\" + filenamedir) : filedir.Name;
                filedir = filedir.Parent;
            }
        }

        /// <summary>
        ///  将目录及其内容拷贝到新位置。
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="targetDirName">目标路径</param>
        public static void Copy(this DirectoryInfo dirInfo, string targetDirName, FileAttributes attributes = FileAttributes.Normal)
        {
            try
            {
                if (!Directory.Exists(targetDirName))
                {
                    Directory.CreateDirectory(targetDirName);

                }
                DirectoryInfo tagInfo = new DirectoryInfo(targetDirName);
                tagInfo.Attributes = attributes;
                string[] dirs = Directory.GetDirectories(dirInfo.FullName);//获取子目录
                FileInfo[] files = dirInfo.GetFiles();//获取子文件文件
                if (dirs.Length > 0)
                {
                    foreach (string dirPath in dirs)
                    {
                        DirectoryInfo dir = new DirectoryInfo(dirPath);
                        if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        {
                            FileAttributes attri = dir.Attributes;
                            string tagDir = System.IO.Path.Combine(targetDirName, dir.Name);
                            dir.Copy(tagDir, attri);
                        }
                    }
                }
                if (files.Length > 0)
                {
                    foreach (FileInfo file in files)
                    {
                        string tagFilePath = System.IO.Path.Combine(targetDirName, file.Name);
                        file.CopyTo(tagFilePath, true);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private static bool checkIg(string path)
        {
            bool b = true;
            foreach (string i in ConfigurationManager.AppSettings["igPath"].Split(','))
            {
                if (path.Contains("\\"+i+"\\"))
                    return false;
            }
            return b;
        }
    }
}
