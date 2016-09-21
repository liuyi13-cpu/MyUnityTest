public class Excel2ClassUtils
{
    public static string FormatString(string name)
    {
        return name.Replace("\r\n", "")
            .Replace("\n", "")
            .Trim();
    }
}


