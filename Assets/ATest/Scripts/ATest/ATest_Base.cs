
using System;

public abstract class ATest_Base : IDisposable
{
    public virtual void Test()
    {
        Debugger.Log("ATest_Base:Test");
    }

    public virtual void Dispose()
    {
    }

    public virtual void UpdateEx()
    {
    }
}