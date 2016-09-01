using System;
using System.Collections.Generic;
using System.Text;

public class ATest_Obj : ATest_Base
{
    // 去警告
#pragma warning disable 414
    int ATest_Obj_int = 1;              // 默认private
#pragma warning restore 414

    protected int ATest_Obj_int1 = 1;
    public int ATest_Obj_int2 { get; set; }     // 属性无变量

    public readonly int money;         // 初始化以后不可修改
    public int Money { get { return money; } }

    public ATest_Obj() { }
    public ATest_Obj(int tmp)
    {
        ATest_Obj_int2 = tmp;
    }

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

    public static List<ATest_Obj> MakeSomeObj()
    {
        return new List<ATest_Obj>
        {
            // new ATest_Obj(),
            new ATest_Obj(0),
            new ATest_Obj(100),
            new ATest_Obj { ATest_Obj_int2 = 100 },
            // new ATest_Obj { money: 100}
        };
    }
}