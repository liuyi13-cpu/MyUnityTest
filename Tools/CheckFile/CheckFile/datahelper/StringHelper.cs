/* 迹I柳燕
 * 
 * FileName:   StringHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 * 
 *========================================
 * @namespace  JilyHelper 
 * @class      StringHelper
 * @extends    
 * 
 *             对于String的扩展方法
 * 
 *========================================
 * Hi,小喵喵...
 * Copyright © 迹I柳燕
 * 
 * 转载请保留...
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CheckFile.dataHelper
{
    public static class StringHelper
    {

        #region 字符编码转换

        /// <summary> string 转换到 byte[] </summary>
        /// <param name="body"> 将要转换的 string </param>
        /// <returns> 返回转换后的 byte[] </returns>
        public static byte[] ConvertToASCII(this string str)
        {
            return new ASCIIEncoding().GetBytes(str);
        }

        public static string ConvertToUTF8(this string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            string retstr = string.Empty;
            buffer.ToList().ForEach(i => retstr += string.Format("%{0:X}", i));
            return retstr;
        }

        public static string ConvertToGB2312(this string str)
        {
            byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(str);
            string retstr = string.Empty;
            buffer.ToList().ForEach(i => retstr += string.Format("%{0:X}", i));
            return retstr;
        }

        #endregion

        //全角转半角
        /// <summary>
        /// vvvv
        /// </summary>
        public const string DoubleByte = "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ～！＠＃＄％︿＆＊（）＿－＋｜＼｛｝［］：＂；＇＜＞，．？／０１２３４５６７８９";
        public const string SingleByte = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_-+|\\{}[]:\";'<>,.?/0123456789";

        public static char ConvertToSingleByte(this char c)
        {
            int index = DoubleByte.IndexOf(c);
            return index != -1 ? SingleByte[c] : c;
        }

        public static char ConvertToDoubleByte(this char c)
        {
            int index = SingleByte.IndexOf(c);
            return index != -1 ? DoubleByte[c] : c;
        }

        public static string ConvertToSingleByte(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append(ConvertToSingleByte(c));
            }
            return sb.ToString();
        }

        public static string ConvertToDoubleByte(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append(ConvertToDoubleByte(c));
            }
            return sb.ToString();
        }

        /// <summary> 替换最后一个匹配字符串 </summary>
        /// <param name="str"> 原字符串 </param>
        /// <param name="oldStr"> 即将被替换的字符串 </param>
        /// <param name="newStr"> 即将替换的字符串, null => string.Empty </param>
        /// <param name="comparisonType"> 字符串检查类型 </param>
        /// <returns> 返回替换后的字符串 </returns>
        public static string ReplaceLast(this string str, string oldStr, string newStr, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase)
        {
            newStr = newStr ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                int index = str.LastIndexOf(oldStr, comparisonType);
                if (index != -1)
                {
                    return str.Remove(index, oldStr.Length).Insert(index, newStr);
                }
            }
            return str;
        }

        public static List<string> Splits(this string str, params string[] args)
        {
            return str.Split(args, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static List<string> SplitsWithTrim(this string str, params string[] args)
        {
            return str.Split(args, StringSplitOptions.RemoveEmptyEntries).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.Trim()).ToList();
        }

        public static string Take(this string str, int startindex, int count)
        {
            return str.Skip(startindex).Take(count).ConverterToString();
        }

        public static string ConverterToString(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }

        /// <summary>
        /// 检测是否包含中文字符
        /// </summary>
        /// <returns></returns>
        public static bool ContainsChineseChar(this string path)
        {
            string fileName = System.IO.Path.GetFileName(path);
            string pattern = "^[\u4e00-\u9fa5]$";
            Regex rx = new Regex(pattern);
            foreach (var item in fileName)
            {
                if (rx.IsMatch(item.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证文件名是否合法
        /// </summary>
        /// <param name="fileName">需要检测的文件名</param>
        /// <returns></returns>
        public static bool CheckForFileName(this string fileName)
        {
            string pattern = @"^[a-zA-Z]:(((\\(?! )[^/:*?<>\""|\\]+)+\\?)|(\\)?)\s*$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch("C:\\" + fileName);
        }

        /// <summary> 是否匹配某一项字符 </summary>
        /// <param name="str"></param>
        /// <param name="comparison"></param>
        /// <param name="parmas"></param>
        /// <returns></returns>
        public static bool IsAnyEqual(this string str, params string[] parmas)
        {
            return IsAnyEqual(str, StringComparison.InvariantCultureIgnoreCase, parmas);
        }

        /// <summary> 是否匹配某一项字符 </summary>
        /// <param name="str"></param>
        /// <param name="comparison"></param>
        /// <param name="parmas"></param>
        /// <returns></returns>
        public static bool IsAnyEqual(this string str, StringComparison comparison, params string[] parmas)
        {
            if (str != null)
            {
                return parmas.FirstOrDefault(item => item != null && item.Equals(str, comparison)) != null;
            }
            return false;
        }

        public static bool IsContins(this string str, params string[] parmas)
        {
            return parmas.All(i => str.ToLower().Contains(i.ToLower()));
        }

    }

}
