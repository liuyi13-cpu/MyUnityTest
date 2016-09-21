using System;
using System.Collections.Generic;
using System.Reflection;

public class DBManager
{
    static Dictionary<string, Dictionary<string, IDB_BaseString>> m_pDBCache = new Dictionary<string, Dictionary<string, IDB_BaseString>>();
    static Dictionary<string, Dictionary<int, IDB_BaseInt>> m_pDBCacheInt = new Dictionary<string, Dictionary<int, IDB_BaseInt>>();

    /// <summary>
    /// 双主键
    /// </summary>
    static Dictionary<string, Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>> m_pDBCacheIntDouble = new Dictionary<string, Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>>();
    static Dictionary<string, Dictionary<string, Dictionary<int, IDB_BaseStringDouble>>> m_pDBCacheStringDouble = new Dictionary<string, Dictionary<string, Dictionary<int, IDB_BaseStringDouble>>>();

    public static string GetText(string key)
    {
        if (key == "") {
            Debugger.LogError("当前key无效：" + key);
            return null;
        }
        return DB_Data_dictionary.getText(key);
    }

    #region 主键string
    /// <summary>
    /// 查询一个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Get<T>(string key) where T : class, IDB_BaseString
    {
        if (key == "")
            return null;
        var type = typeof(T);
        if (!m_pDBCache.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCache[type.Name] = info.Invoke(null, null) as Dictionary<string, IDB_BaseString>;
        }
        var subCache = m_pDBCache[type.Name];
        T model = null;
        try {
            model = subCache[key] as T;
        } catch (Exception e) {
            Debugger.LogError("Get: " + e.Message + ", key: " + key);
        }
        return model;
    }

    /// <summary>
    /// 一次获取string类型主键表中所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<string, T> GetStringKeyALL<T>() where T : class, IDB_BaseString
    {
        var type = typeof(T);
        if (!m_pDBCache.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCache[type.Name] = info.Invoke(null, null) as Dictionary<string, IDB_BaseString>;
        }
        Dictionary<string, T> dicts = new Dictionary<string, T>();
        Dictionary<string, IDB_BaseString>.Enumerator enumerator = m_pDBCache[type.Name].GetEnumerator();
        while (enumerator.MoveNext())
        {
            dicts.Add(enumerator.Current.Key, enumerator.Current.Value as T);
        }
        return dicts;
    }

    #endregion

    #region 主键int
    /// <summary>
    /// 查询一个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Get<T>(int key) where T : class, IDB_BaseInt
    {
        if (key == -1)
            return null;
        var type = typeof(T);
        if (!m_pDBCacheInt.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheInt[type.Name] = info.Invoke(null, null) as Dictionary<int, IDB_BaseInt>;
        }
        var subCache = m_pDBCacheInt[type.Name];
        T model = null;
        try {
            model = subCache[key] as T;
        } catch (Exception e) {
            Debugger.LogError("Get: " + e.Message + ", key: " + key);
        }
        return model;
    }

    /// <summary>
    /// 一次获取int类型主键表中所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<int, T> GetIntKeyALL<T>() where T : class, IDB_BaseInt
    {
        var type = typeof(T);
        if (!m_pDBCacheInt.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheInt[type.Name] = info.Invoke(null, null) as Dictionary<int, IDB_BaseInt>;
        }
        Dictionary<int, T> dicts = new Dictionary<int, T>();
        Dictionary<int, IDB_BaseInt>.Enumerator enumerator = m_pDBCacheInt[type.Name].GetEnumerator();
        while (enumerator.MoveNext())
        {
            dicts.Add(enumerator.Current.Key, enumerator.Current.Value as T);
        }
        return dicts;
    }

    #endregion

    #region 双主键int,int
    /// <summary>
    /// 双主键查询一个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static T Get<T>(int key, int level) where T : class, IDB_BaseIntDouble
    {
        if (key == -1 || level == -1)
            return null;
        var type = typeof(T);
        if (!m_pDBCacheIntDouble.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheIntDouble[type.Name] = info.Invoke(null, null) as Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>;
        }
        var subCache = m_pDBCacheIntDouble[type.Name];
        T model = null;
        try {
            model = subCache[key][level] as T;
        } catch (Exception e) {
            Debugger.LogError("Get: " + e.Message + ", key: " + key + ",level: " + level);
        }
        return model;
    }

    /// <summary>
    /// 一次获取int类型主键表中某个key的所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<int, T> GetDoubleIntKeyALL<T>(int key) where T : class, IDB_BaseIntDouble
    {
        if (key == -1)
            return null;
        var type = typeof(T);
        if (!m_pDBCacheIntDouble.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheIntDouble[type.Name] = info.Invoke(null, null) as Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>;
        }
        Dictionary<int, T> dicts = new Dictionary<int, T>();
        Dictionary<int, IDB_BaseIntDouble>.Enumerator enumerator = m_pDBCacheIntDouble[type.Name][key].GetEnumerator();
        while (enumerator.MoveNext())
        {
            dicts.Add(enumerator.Current.Key, enumerator.Current.Value as T);
        }
        return dicts;
    }

    /// <summary>
    /// 一次获取int类型主键表中所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<int, Dictionary<int, T>> GetDoubleIntKeyALL<T>() where T : class, IDB_BaseIntDouble
    {
        var type = typeof(T);
        if (!m_pDBCacheIntDouble.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheIntDouble[type.Name] = info.Invoke(null, null) as Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>;
        }

        Dictionary<int, Dictionary<int, T>> dicts = new Dictionary<int, Dictionary<int, T>>();

        Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>.Enumerator enumerator = m_pDBCacheIntDouble[type.Name].GetEnumerator();
        while (enumerator.MoveNext())
        {
            Dictionary<int, T> itemDict = new Dictionary<int, T>();
            Dictionary<int, IDB_BaseIntDouble>.Enumerator enumerator2 = enumerator.Current.Value.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                itemDict.Add(enumerator2.Current.Key, enumerator2.Current.Value as T);
            }
            dicts.Add(enumerator.Current.Key, itemDict);
        }

        return dicts;
    }

    #endregion

    #region 双主键string,int
    /// <summary>
    /// 双主键查询一个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static T Get<T>(string key, int level) where T : class, IDB_BaseStringDouble
    {
        if (key == null || key.Equals("") || level == -1)
            return null;

        var type = typeof(T);
        if (!m_pDBCacheStringDouble.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheStringDouble[type.Name] = info.Invoke(null, null) as Dictionary<string, Dictionary<int, IDB_BaseStringDouble>>;
        }
        var subCache = m_pDBCacheStringDouble[type.Name];
        T model = null;
        try
        {
            model = subCache[key][level] as T;
        }
        catch (Exception e)
        {
            Debugger.LogError("Get: " + e.Message + ", key: " + key + ",level: " + level);
        }
        return model;
    }

    /// <summary>
    /// 一次获取int类型主键表中某个key的所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<int, T> GetDoubleStringKeyALL<T>(string key) where T : class, IDB_BaseStringDouble
    {
        if (key == null || key.Equals(""))
            return null;

        var type = typeof(T);
        if (!m_pDBCacheStringDouble.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheStringDouble[type.Name] = info.Invoke(null, null) as Dictionary<string, Dictionary<int, IDB_BaseStringDouble>>;
        }

        Dictionary<int, T> dicts = new Dictionary<int, T>();

        Dictionary<int, IDB_BaseStringDouble>.Enumerator enumerator = m_pDBCacheStringDouble[type.Name][key].GetEnumerator();
        while (enumerator.MoveNext())
        {
            dicts.Add(enumerator.Current.Key, enumerator.Current.Value as T);
        }

        return dicts;
    }

    /// <summary>
    /// 一次获取int类型主键表中所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<string, Dictionary<int, T>> GetDoubleStringKeyALL<T>() where T : class, IDB_BaseStringDouble
    {
        var type = typeof(T);
        if (!m_pDBCacheStringDouble.ContainsKey(type.Name))
        {
            MethodInfo info = type.GetMethod("LoadDB", BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Debugger.LogError("DBManager  : " + type.Name);
                return null;
            }

            m_pDBCacheStringDouble[type.Name] = info.Invoke(null, null) as Dictionary<string, Dictionary<int, IDB_BaseStringDouble>>;
        }

        Dictionary<string, Dictionary<int, T>> dicts = new Dictionary<string, Dictionary<int, T>>();

        Dictionary<string, Dictionary<int, IDB_BaseStringDouble>>.Enumerator enumerator = m_pDBCacheStringDouble[type.Name].GetEnumerator();
        while (enumerator.MoveNext())
        {
            Dictionary<int, T> itemDict = new Dictionary<int, T>();

            Dictionary<int, IDB_BaseStringDouble>.Enumerator enumerator2 = enumerator.Current.Value.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                itemDict.Add(enumerator2.Current.Key, enumerator2.Current.Value as T);
            }
            dicts.Add(enumerator.Current.Key, itemDict);
        }

        return dicts;
    }

    #endregion

    /// <summary>
    /// 一次得到多个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static List<T> Get<T>(List<int> keys) where T : class, IDB_BaseInt {
        List<T> list = new List<T>();
        int count = keys.Count;
        for (int i = 0; i < count; i++) {
            T t = Get<T>(keys[i]);
            list.Add(t);
        }
        return list;
    }

    /// <summary>
    /// 一次得到多个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static List<T> Get<T>(List<string> keys) where T : class, IDB_BaseString {
        List<T> list = new List<T>();
        int count = keys.Count;
        for (int i = 0; i < count; i++) {
            T t = Get<T>(keys[i]);
            list.Add(t);
        }
        return list;
    }
}
