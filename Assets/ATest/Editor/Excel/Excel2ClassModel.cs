using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Excel2ClassModel
{
    /// <summary>
    /// 双主键表
    /// </summary>
    static string[] doublePrimaryKeyFiles =
    {
        "conf_activities_ui_pos_client",
        "conf_commander_tactic_client",
        "conf_skill_info_public",  // 技能表
        "conf_buff_info_client",   // buff表
        "conf_rare_performance_public",
        "conf_strategy_arsenal_public",
        "conf_strategy_fortress_public",
        "conf_strategy_researchcenter_public",
        "conf_strategy_zone_link_public",
        "conf_airport_reward_public",
        "conf_res_effect_info_client",
        "conf_equip_recast_public",
    };

    const int COLUMN_KEY = 2;        // 主键列   
    const int COLUMN_KEY_SUB = 3;   // 副主键列

    const int ROW_DES = 1;        // 注释
    const int ROW_NAME = 2;        // 属性名
    const int ROW_TYPE = 3;        // 类型

    List<string> listDes = new List<string>();
    List<string> listName = new List<string>();
    List<string> listType = new List<string>();

    string class_name;      // 类名
    string table_name;      // 表名
    string parent_name;     // 父类
    string parent_name1;
    string parent_name2;

    bool isDoubleKey;          // 双主键
    bool isStringKey;          // string作为第一个主键

    string excel_name;

    /*
    // 主键列值
    List<int> listKeyInt = new List<int>();
    List<string> listKeyStr = new List<string>();
    */

    public void Export(string name, ProcessType type)
    {
        excel_name = name;

        if (string.IsNullOrEmpty(name)) return;

        bool valid = checkClassName(name);
        if (!valid) return;

        checkTabelName(name);

        // 解析xlsx
        using (var fs = new FileStream(name, FileMode.Open))
        {
            try
            {
                ExcelPackage pack = new ExcelPackage(fs);
                if (pack.Workbook.Worksheets.Count > 0)
                {
                    ExcelWorksheet workSheet = pack.Workbook.Worksheets[1];
                    CheckPrimaryKey(workSheet);

                    switch (type)
                    {
                        case ProcessType.Excel2Class:
                            // 生成类
                            ParseXlsx(workSheet);
                            CreateClass(JoinClass());
                            break;

                        case ProcessType.CheckPrimaryKey:
                            ParsePrimaryKey(workSheet);
                            break;

                        default:
                            break;
                    }
                }
                fs.Close();
            }
            catch (System.Exception)
            {
                Debugger.LogError(excel_name);
                throw;
            }
        }
    }

    bool checkClassName(string name)
    {
        int length = 0;
        int startIndex = name.LastIndexOf("/conf_") + 6;
        if (name.Contains("_public"))
        {
            length = name.LastIndexOf("_public.xlsx") - startIndex;
        }
        else if (name.Contains("_client"))
        {
            length = name.LastIndexOf("_client.xlsx") - startIndex;
        }
        else
        {
            Debugger.LogWarning("无效的文件：" + name);
            return false;
        }

        name = name.Substring(startIndex, length);
        class_name = "DB_" + UpFirstChar(name);

        return true;
    }

    void checkTabelName(string name)
    {
        int length = 0;
        int startIndex = name.LastIndexOf("/") + 1;
        if (name.Contains("_public"))
        {
            length = name.LastIndexOf("_public.xlsx") - startIndex;
        }
        else if (name.Contains("_client"))
        {
            length = name.LastIndexOf("_client.xlsx") - startIndex;
        }
        else
        {
            Debugger.LogError(name);
            return;
        }
        table_name = name.Substring(startIndex, length);

        isDoubleKey = isDoublePrimaryKey(name);
    }

    /// <summary>
    /// 是否双主键表
    /// </summary>
    bool isDoublePrimaryKey(string name)
    {
        foreach (var item in doublePrimaryKeyFiles)
        {
            if (name.IndexOf(item) != - 1)
            {
                return true;
            }
        }
        return false;
    }

    string UpFirstChar(string name)
    {
        return name.Substring(0, 1).ToUpper() + name.Substring(1);
    }

    // 读取xlsx
    void ParseXlsx(ExcelWorksheet workSheet)
    {
        int columns = workSheet.Dimension.Columns;

        for (int i = 1; i <= columns; i++)
        {
            string des = Excel2ClassUtils.FormatString(workSheet.GetValue<string>(ROW_DES, i));
            listDes.Add(des);

            string name = workSheet.GetValue<string>(ROW_NAME, i);
            listName.Add(name);

            string type = CheckType(workSheet.GetValue<string>(ROW_TYPE, i));
            listType.Add(type);
        }
    }

    string CheckType(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Equals("INT_PK"))
        {
            return "int";
        }
        else if (name.Equals("DOUBLE"))
        {
            return "float";
        }
        else
        {
            return name.ToLower();
        }
    }

    // 生成类
    void CreateClass(StringBuilder builder)
    {
        string OutDir = Application.dataPath + "/Scripts/DB/DBModel/";

        if (!Directory.Exists(OutDir))
        {
            Directory.CreateDirectory(OutDir);
        }

        using (var fs = new FileStream(OutDir + class_name + ".cs", FileMode.Create))
        {
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(builder.ToString());
                sw.Flush();
                sw.Close();
            }
            fs.Close();
        }
    }

    // 拼接类
    StringBuilder JoinClass()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("using System.Collections.Generic; ").Append("\r\n").Append("\r\n");
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!");
        builder.AppendLine("/// </summary>");

        builder.Append("public class ").Append(class_name).Append(" : ").Append(parent_name).Append("\r\n");
        builder.Append("{").Append("\r\n");
        builder.Append("\t").Append("const string SQL = @\"select ");
        foreach (var item in listName)
        {
            builder.Append(item).Append(", ");
        }
        builder.Remove(builder.Length - 2, 2);
        builder.Append(" from ").Append(table_name).Append("\";").Append("\r\n").Append("\r\n");

        for (int i = 0; i < listType.Count; i++)
        {
            builder.Append("\t").Append("/// <summary>\r\n").Append("\t///").Append(listDes[i]).Append("\r\n\t/// </summary>\r\n");
            builder.Append("\t").Append("public ").Append(listType[i]).Append(" ").Append(UpFirstChar(listName[i])).Append(" { get; set; }").Append("\r\n");
        }

        builder.Append("\r\n");

        builder.Append("\t").Append("static ").Append(parent_name1).Append(" LoadDB()").Append("\r\n");
        builder.Append("\t").Append("{").Append("\r\n");

        builder.Append("\t\t").Append(parent_name1).Append(" tmp = new ").Append(parent_name1).Append("();").Append("\r\n");
        builder.Append("\t\t").Append("DBHelper.Instance.Query(SQL, (reader) =>").Append("\r\n");
        builder.Append("\t\t").Append("{").Append("\r\n");

        builder.Append("\t\t\t").Append("if (reader == null) return;").Append("\r\n").Append("\r\n");
        builder.Append("\t\t\t").Append("while (reader.Read())").Append("\r\n");
        builder.Append("\t\t\t").Append("{").Append("\r\n");
        builder.Append("\t\t\t\t").Append(class_name).Append(" module = new ").Append(class_name).Append("()").Append("\r\n");
        builder.Append("\t\t\t\t").Append("{").Append("\r\n");

        for (int i = 0; i < listType.Count; i++)
        {
            builder.Append("\t\t\t\t\t").Append(UpFirstChar(listName[i])).Append("\t\t = reader.").Append(checkTypeFunction(listType[i])).Append(i).Append("),").Append("\r\n");
        }

        builder.Append("\t\t\t\t").Append("};").Append("\r\n");

        if (isDoubleKey)
        {
            builder.Append("\t\t\t\t").Append("if (!tmp.ContainsKey(module.").Append(UpFirstChar(listName[1])).Append("))").Append("\r\n");
            builder.Append("\t\t\t\t").Append("{").Append("\r\n");
            builder.Append("\t\t\t\t\t").Append("var tmpsub = new ").Append(parent_name2).Append("();").Append("\r\n");
            builder.Append("\t\t\t\t\t").Append("tmp.Add(module.").Append(UpFirstChar(listName[1])).Append(", tmpsub);").Append("\r\n");
            builder.Append("\t\t\t\t").Append("}").Append("\r\n");
            builder.Append("\t\t\t\t").Append("tmp[module.").Append(UpFirstChar(listName[1])).Append("].Add(module.").Append(UpFirstChar(listName[2])).Append(", module);").Append("\r\n");
        }
        else
        {
            builder.Append("\t\t\t\t").Append("tmp.Add(module.").Append(UpFirstChar(listName[1])).Append(", module);").Append("\r\n");
        }
        
        builder.Append("\t\t\t").Append("}").Append("\r\n");
        builder.Append("\t\t").Append("});").Append("\r\n");
        builder.Append("\t\t").Append("return tmp;").Append("\r\n");
        builder.Append("\t").Append("}").Append("\r\n");
        builder.Append("}").Append("\r\n");

        return builder;
    }

    string checkTypeFunction(string name)
    {
        if (name.Equals("int"))
        {
            return "GetInt32(";
        }
        else if (name.Equals("float"))
        {
            return "GetFloat(";
        }
        else
        {
            return "GetString(";
        }
    }

    void CheckPrimaryKey(ExcelWorksheet workSheet)
    {
        // 列数
        int columns = workSheet.Dimension.Columns;
        // 使用第二列作为Key
        if (columns < COLUMN_KEY)
        {
            Debugger.LogError(excel_name);
        }

        string var = workSheet.GetValue<string>(ROW_TYPE, COLUMN_KEY);
        isStringKey = var.Trim().Equals("STRING");

        if (isDoubleKey)
        {
            if (isStringKey)
            {
                parent_name = "IDB_BaseStringDouble";
                parent_name1 = "Dictionary<string, Dictionary<int, IDB_BaseStringDouble>>";
                parent_name2 = "Dictionary<int, IDB_BaseStringDouble>";
            }
            else
            {
                parent_name = "IDB_BaseIntDouble";
                parent_name1 = "Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>";
                parent_name2 = "Dictionary<int, IDB_BaseIntDouble>";
            }
        }
        else
        {
            
            if (isStringKey)
            {
                parent_name = "IDB_BaseString";
                parent_name1 = "Dictionary<string, IDB_BaseString>";
            }
            else
            {
                parent_name = "IDB_BaseInt";
                parent_name1 = "Dictionary<int, IDB_BaseInt>";
            }
        }
    }

    /// <summary>
    /// 读取主键列
    /// </summary>
    void ParsePrimaryKey(ExcelWorksheet workSheet)
    {
        List<string> keyListString = new List<string>();
        StringBuilder builder = new StringBuilder();
        builder.Append("<color=red>");
        builder.AppendLine(excel_name);

        bool exist = false;
        // 行数
        int rows = workSheet.Dimension.Rows;
        for (int i = 5; i <= rows; i++)
        {
            var key = workSheet.GetValue<string>(i, COLUMN_KEY);

            if (isDoubleKey)
            {
                key += workSheet.GetValue<string>(i, COLUMN_KEY_SUB);
            }
            if (keyListString.Contains(key))
            {
                // 重复
                builder.Append("重复的KEY: ").Append(key).Append("第").Append(i).AppendLine("行");
                exist = true;
            }
            else
            {
                keyListString.Add(key);
            }
        }

        builder.Append("</color>");
        if (exist)
        {
            Debugger.Log(builder.ToString());
        }
    }
}


