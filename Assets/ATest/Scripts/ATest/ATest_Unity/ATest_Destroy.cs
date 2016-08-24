using System.Collections;
using UnityEngine;

namespace Gc.Game.Test
{
    /// <summary>
    /// 资源，代码 内存申请和释放
    /// http://www.jianshu.com/p/b37ee8cea04c
    /// </summary>
    public class ATest_Destroy : MonoBehaviour
    {
        public  void Start()
        {
            Debugger.Log("ATest_Destroy:Test");

            StartCoroutine(LoadResources());
        }

        IEnumerator LoadResources()
        {
            // 资源创建过程
            // load prefab - Instantiate

            // Resources.Load();
            // Resources.UnloadAsset(Object);       // 释放目标Asset对象 
            Resources.UnloadUnusedAssets();         // 释放所有没有引用的Asset对象

            var ab = AssetBundle.LoadFromFile("path");
            var prefab = ab.LoadAsset<AudioClip>("name");
            ab.Unload(false);                       // 释放AB文件内存镜像，不销毁Load创建的Assets对象
            ab.Unload(true);                        // 释放AB文件内存镜像同时销毁所有已经Load的Assets内存镜像

            // GameObject
            // Instantiate  - GameObject.Destroy(X)

            yield return null;
        }


        void TestGC()
        {
            Utils.ClearMemory();
        }
    }
}