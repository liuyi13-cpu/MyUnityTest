using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏入口
/// 需要确保Main一个执行
/// </summary>
public class Main : MonoBehaviour
{
    static Main s_Instance = null;
    List<Base> m_ListMgr = new List<Base>();

    // 网络Manager
    public NetworkManager m_NetMgr;

    public static Main Instance
    {
        get
        {
            return s_Instance;
        }
    }

    void Start()
    {
        s_Instance = this;

        _AddManager();

        var length = m_ListMgr.Count;
        for (int i = 0; i < length; i++)
        {
            m_ListMgr[i].StartEx();
        }
    }

    void _AddManager()
    {
        m_NetMgr = new NetworkManager();
        m_ListMgr.Add(m_NetMgr);

        // TODO
    }

    void FixedUpdate()
    {
        var length = m_ListMgr.Count;
        for (int i = 0; i < length; i++)
        {
            m_ListMgr[i].FixedUpdateEx();
        }
    }

    void Update()
    {
        var length = m_ListMgr.Count;
        for (int i = 0; i < length; i++)
        {
            m_ListMgr[i].UpdateEx();
        }
    }

    void LateUpdate()
    {
        var length = m_ListMgr.Count;
        for (int i = 0; i < length; i++)
        {
            m_ListMgr[i].LateUpdateEx();
        }
    }

    void OnDestroy()
    {
        var length = m_ListMgr.Count;
        for (int i = 0; i < length; i++)
        {
            m_ListMgr[i].OnDestroyEx();
        }
    }
}