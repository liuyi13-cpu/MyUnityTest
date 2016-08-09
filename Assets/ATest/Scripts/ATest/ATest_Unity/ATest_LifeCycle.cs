using System;
using System.Text;
using UnityEngine;

namespace Gc.Game.Test
{
    public class ATest_LifeCycle : MonoBehaviour
    {
        void Reset()
        {
            // Editor模式下 添加脚本或者右键调用Reset命令
            Debugger.Log("ATest_LifeCycle:Reset");
        }

        void Awake()
        {
            Debugger.Log("ATest_LifeCycle:Awake");
            
            // goto OnDisable()
            // this.enabled = false;
            // else
            // goto OnEnable()
        }

        void OnEnable()
        {
            Debugger.Log("ATest_LifeCycle:OnEnable");
        }
        void Start()
        {
            Debugger.Log("ATest_LifeCycle:Start");

            // OnDisable-OnDestroy
            // Destroy(this);
        }

        void OnDisable()
        {
            Debugger.Log("ATest_LifeCycle:OnDisable");
        }

        void FixedUpdate()
        {
            Debugger.Log("ATest_LifeCycle:FixedUpdate" + DateTime.Now + "   " + DateTime.Now.Millisecond);
        }

        void Update()
        {
            // Application.targetFrameRate = 10;
            Debugger.Log("ATest_LifeCycle:Update" + DateTime.Now + "   " + DateTime.Now.Millisecond);

//             string str = "";
//             for (int i = 0; i < 20000; i++)
//             {
//                 str += "1";
//             }
        }

        void LateUpdate()
        {
            Debugger.Log("ATest_LifeCycle:LateUpdate" + DateTime.Now + "   " + DateTime.Now.Millisecond);
        }

//         void OnGUI()
//         {
//             Debugger.Log("ATest_LifeCycle:OnGUI");
//         }

        void OnDestroy()
        {
            Debugger.Log("ATest_LifeCycle:OnDestroy");
        }
    }
}