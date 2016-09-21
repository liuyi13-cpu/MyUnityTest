using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 保持 http server的cookie
/// </summary>
public static class HttpCookie
{
    public static string currentCookie;

    public static void Reset()
    {
        currentCookie = null;
    }
}
