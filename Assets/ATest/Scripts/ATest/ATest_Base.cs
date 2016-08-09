
using System;

namespace Gc.Game.Test
{
    public abstract class ATest_Base : IDisposable
    {
        public virtual void Test()
        {
            Debugger.Log("ATest_Base:Test");
        }

        public virtual void Dispose()
        {
        }
    }
}