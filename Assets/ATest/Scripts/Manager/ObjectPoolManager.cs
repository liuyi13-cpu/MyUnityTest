using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// 对象池管理器，普通类对象池
/// </summary>
public class ObjectPoolManager : Base
{
    Dictionary<string, object> m_ObjectPools = new Dictionary<string, object>();

    public T Get<T>() where T : class, new()
    {
        var pool = _GetPool<T>();
        if (pool != null)
        {
            return pool.Get();
        }
        return default(T);
    }

    public void Release<T>(T obj) where T : class, new()
    {
        var pool = _GetPool<T>();
        if (pool != null)
        {
            pool.Release(obj);
        }
    }

    public ObjectPool<T> CreatePool<T>(UnityAction<T> actionOnGet = null, UnityAction<T> actionOnRelease = null) where T : class, new()
    {
        var type = typeof(T);
        var pool = new ObjectPool<T>(actionOnGet, actionOnRelease);
        m_ObjectPools[type.Name] = pool;
        return pool;
    }

    ObjectPool<T> _GetPool<T>() where T : class, new()
    {
        var type = typeof(T);
        ObjectPool<T> pool = null;
        if (m_ObjectPools.ContainsKey(type.Name))
        {
            pool = m_ObjectPools[type.Name] as ObjectPool<T>;
        }
        else
        {
            pool = CreatePool<T>();
        }
        return pool;
    }
}