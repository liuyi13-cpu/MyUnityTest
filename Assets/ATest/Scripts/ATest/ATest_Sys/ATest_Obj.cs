using System;
using System.Text;

namespace Gc.Game.Test
{
    public class ATest_Obj : ATest_Base
    {
        // 去警告
#pragma warning disable 414
        int ATest_Obj_int = 1;              // 默认private
#pragma warning restore 414

        protected int ATest_Obj_int1 = 1;
        public int ATest_Obj_int2 { get; set; }

        public override void Test()
        {
            // 调用父类的方法
            base.Test();
            Debugger.Log("ATest_Obj:Test");
        }
    }

    public class ATest_Obj_sub : ATest_Obj
    {
        public override void Test()
        {
            // Debugger.Log("ATest_Obj_sub:Test");

            // 调用父类的方法
            base.Test();
            Debugger.Log("ATest_Obj_sub:Test");

            // ATest_Obj_int = 2;
            ATest_Obj_int1 = 2;
            ATest_Obj_int2 = 2;
        }
    }
}