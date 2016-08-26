using UnityEngine;
using System.Collections;
using System.Text;
using System;
using Gc.Game.Test;

public class ATest : MonoBehaviour
{
    // 测试类型
    enum TestType
    {
        TestType_Lua,
        TestType_Shader,    // shader
        TestType_AB,        // ab
        TestType_LifeCycle, // 脚本生命周期测试
        TestType_Macro,     // 宏定义测试
        TestType_Obj,       // 面向对象
        TestType_Ref,       // 引用测试
        TestType_String,    // 字符串测试
    }

    // Use this for initialization
    void Start()
    {
        // TestDebug();
        // Test_unsafe();
        TestType testType = TestType.TestType_Macro;
        ATest_Base test = null;
        switch (testType)
        {
            case TestType.TestType_Lua:
                test = new ATest_Lua();
                break;

            case TestType.TestType_Shader:
                test = new ATest_Shader();
                break;

            case TestType.TestType_AB:
                test = new ATest_AssetBundle();
                break;

            case TestType.TestType_LifeCycle:
                // test = new ATest_LifeCycle();
                break;

            case TestType.TestType_Macro:
                test = new ATest_Macro();
                break;

            case TestType.TestType_Obj:
                test = new ATest_Obj_sub();
                break;

            case TestType.TestType_Ref:
                test = new ATest_Ref();
                break;

            case TestType.TestType_String:
                test = new ATest_String();
                break;

            default:
                break;
        }

        if (test != null)
        {
            test.Test();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TestDebug()
    {
        bool debug = Debug.developerConsoleVisible;
        Debugger.Log(debug);

        Debug.developerConsoleVisible = true;
        Debugger.Log(Debug.developerConsoleVisible);
    }

    // 指针测试
    // 只能取值类型的地址
    void Test_unsafe()
    {
        //             unsafe
        //             {
        //                 string str1;
        //                 str1 = "str1";
        //                 str1 += "str2";
        // 
        //                 string str2 = "dadf";
        //                 int* addrest1 = &str2;
        // 
        //                 TestRef(str1);
        // 
        // 
        //                 int aaa = 1;
        //                 int* addrest = &aaa;
        //             }
    }

    void TestLog()
    {
        string str = "Test Log 1 Test Log 2";
        Debugger.Log(str);
        Debugger.LogWarning(str);
        Debugger.LogError(str);
    }
}