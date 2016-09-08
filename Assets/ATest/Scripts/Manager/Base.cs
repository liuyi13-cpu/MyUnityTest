using UnityEngine;
using System.Collections;
using System;

public class Base : IDisposable
{

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

    public virtual void Dispose()
    {
        throw new NotImplementedException();
    }
}
