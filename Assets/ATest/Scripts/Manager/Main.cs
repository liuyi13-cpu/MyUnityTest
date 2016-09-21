using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ���
/// ��Ҫȷ��Mainһ��ִ��
/// </summary>
public class Main : MonoBehaviour
{
    List<Base> m_ListMgr = new List<Base>();

    /// <summary>
    /// ����Manager
    /// </summary>
    public AudioManager         m_AudioMgr;
    /// <summary>
    /// ����Manager
    /// </summary>
    public NetManager           m_NetMgr;
    /// <summary>
    /// ��ͨ�����
    /// </summary>
    public ObjectPoolManager    m_PoolMgr;
    /// <summary>
    /// ��ʾ��Ϣ
    /// </summary>
    public TipManager           m_TipMgr;

    static Main s_Instance = null;
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
        var _rootAudio = new GameObject("AudioRoot");
        _rootAudio.transform.parent = transform;
        m_AudioMgr = new AudioManager(_rootAudio.transform);
        m_ListMgr.Add(m_AudioMgr);

        m_NetMgr = new NetManager(new NetManagerControl());
        m_ListMgr.Add(m_NetMgr);

        m_PoolMgr = new ObjectPoolManager();
        // m_ListMgr.Add(m_PoolMgr);

        var _rootTip = new GameObject("TipRoot");
        _rootTip.transform.parent = transform;
        m_TipMgr = new TipManager(_rootTip.transform);
        m_ListMgr.Add(m_TipMgr);

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