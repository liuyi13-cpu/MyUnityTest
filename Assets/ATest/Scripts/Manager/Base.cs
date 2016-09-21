using UnityEngine;

public class Base
{
    /// <summary>
    /// 父节点
    /// </summary>
    protected Transform m_Root;

    protected Base(Transform parent = null)
    {
        m_Root = parent;
    }

    // Use this for initialization
    public virtual void StartEx()
    {
    }

    public virtual void FixedUpdateEx()
    {
    }

    public virtual void UpdateEx()
    {
    }

    public virtual void LateUpdateEx()
    {
    }

    public virtual void OnDestroyEx()
    {
    }
}
