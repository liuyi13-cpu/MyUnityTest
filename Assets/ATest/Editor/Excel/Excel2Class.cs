using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

public enum ProcessType
{
    Excel2Class,
    CheckPrimaryKey
}

public class Excel2Class : Editor
{
    /// <summary>
    /// 导出目录
    /// </summary>
    static string[] targetDirectory =
    {
        @"Docs/ConfTable/public/",
        @"Docs/ConfTable/client/"
    };

    /// <summary>
    /// 过滤目录
    /// </summary>
    static string[] filterFiles =
    {
        "conf_data_dictionary_I18N",
    };

    [MenuItem("Excel/Excel ALL", false, 1)]
    static void ExcelALL()
    {
        Excel2Class_();
        CheckPrimaryKey();
        MakeTextConstClass();
        CheckTextConstClass();
        MakeErrorMessageConstClass();
    }

    [MenuItem("Excel/Excel2Class")]
    static void Excel2Class_()
    {
        _Process("生成DB类", ProcessType.Excel2Class);
    }

    [MenuItem("Excel/Excel检查DB重复主键")]
    static void CheckPrimaryKey()
    {
        _Process("检查DB重复主键", ProcessType.CheckPrimaryKey);
    }

    [MenuItem("Excel/导出Text的常量类")]
    static void MakeTextConstClass()
    {
        var model = new SQL2TextConstClassModel();
        model.Export();
    }

    [MenuItem("Excel/检查Text的常量类")]
    static void CheckTextConstClass()
    {
        var model = new SQL2TextConstClassModel();
        model.CheckExport();
    }

    [MenuItem("Excel/导出ErrorMessage的常量类")]
    static void MakeErrorMessageConstClass()
    {
        var model = new SQL2ErrorMessageClassModel();
        model.Export();
    }

    static void _Process(string title, ProcessType type)
    {
        int startIndex = 0;
        float total = 0;
        List<string> files = new List<string>();

        foreach (var item in targetDirectory)
        {
            string[] filesSub = Directory.GetFiles(item, "*.xlsx");
            total += filesSub.Length;
            files.AddRange(filesSub);
        }

        foreach (var file in files)
        {
            if (isInvalidFile(file))
            {
                startIndex++;
                continue;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(title).Append(startIndex).Append("/").Append(total);

            bool isCancel = EditorUtility.DisplayCancelableProgressBar(builder.ToString(), file, (float)startIndex / total);
            if (isCancel)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            startIndex++;

            try
            {
                Excel2ClassModel model = new Excel2ClassModel();
                model.Export(file, type);
            }
            catch (System.Exception)
            {
                EditorUtility.ClearProgressBar();
                throw;
            }
        }

        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 无效文件
    /// </summary>
    static bool isInvalidFile(string name)
    {
        foreach (var item in filterFiles)
        {
            if (name.IndexOf(item) != -1)
            {
                return true;
            }
        }
        return false;
    }
}


