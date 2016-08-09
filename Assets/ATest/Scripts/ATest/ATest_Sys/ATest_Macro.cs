#if !TEST_MACRO
#define TEST_MACRO
#endif

#define TEST_MACRO1

using System;
using System.Text;
using UnityEngine;

// #define TEST_MACRO1 /// ERR

namespace Gc.Game.Test
{
    // #define TEST_MACRO2 /// ERR

    // 1 #region 功能分块
    // 2 #define 只能用作宏开关 只能在using之前
    public class ATest_Macro : ATest_Base
    {
    #region region
        #region function1
        public override void Test()
        {
            base.Test();
            Debugger.Log("ATest_macro override Test()");
            TestDefine();

        }
        #endregion function1

        #region function2
       
        void Function2()
        {
            Debugger.Log("Function2");
        }
        #endregion function2
    #endregion region

    #region define
        #if TEST_MACRO
        // #define MAX_NUM 100 /// ERR
        const int MAX_NUM = 100;
        // 
        #else
        // #define MAX_NUM  /// ERR
        #endif
    
        void TestDefine()
        {
            string platform = "";
#if UNITY_EDITOR
            platform = "hi,大家好,我是在unity编辑模式下";
#elif UNITY_XBOX360  
       platform="hi，大家好,我在XBOX360平台";  
#elif UNITY_IPHONE  
       platform="hi，大家好,我是IPHONE平台";  
#elif UNITY_ANDROID  
       platform="hi，大家好,我是ANDROID平台";  
#elif UNITY_STANDALONE_OSX  
       platform="hi，大家好,我是OSX平台";  
#elif UNITY_STANDALONE_WIN  
       platform="hi，大家好,我是Windows平台";  
#endif
            Debugger.Log("Current Platform:" + platform);
            Debugger.Log("Current Platform:" + Application.platform);
            Debugger.Log("Current version:" + Application.version);
            Debugger.Log("Current unityVersion:" + Application.unityVersion);

            string version = "";

#if UNITY_5_2_2
            version = "hi，大家好,我是Unity5.2.2";
#elif UNITY_5_3
            version = "hi，大家好,我是Unity5.3";
#endif
            Debugger.Log("Current Version:" + version);

            // 脚本编译器
#if ENABLE_DOTNET
            version = "hi，大家好,我是DOTNET";
#elif ENABLE_MONO
            version = "hi，大家好,我是MONO";
#elif ENABLE_IL2CPP
            version = "hi，大家好,我是IL2CPP";
#endif
            Debugger.Log("Scripting backend:" + version);

#if DEVELOPMENT_BUILD
            Debugger.Log("DEVELOPMENT_BUILD");
#endif
        }
    #endregion define
    }
}