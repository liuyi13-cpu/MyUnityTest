using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 资源，代码 内存申请和释放
/// http://www.jianshu.com/p/b37ee8cea04c
/// </summary>
public class ATest_Destroy : MonoBehaviour
{
    void Start()
    {
        Debugger.Log("ATest_Destroy:Test");

        // StartCoroutine(LoadResources());
        StartCoroutine(TestDestroy());
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


    List<GameObject> list = new List<GameObject>();

    IEnumerator TestDestroy()
    {
        // 没有parent的obj会在当前scene的根目录下
        var root = new GameObject("Root");
        int type = 0;
        switch (type)
        {
            case 0:
                // 保存静态引用
                DontDestroyOnLoad(root);
                var componet = root.AddComponent<ATest_Destroy_1>();

                
                var root2 = new GameObject("Root2");
                DontDestroyOnLoad(root2);

                list.Add(root);
                list.Add(root2);

                ATest_Destroy_1.s_obj = root2;

                DestroyImmediate(componet);

                Debugger.LogWarning(ATest_Destroy_1.s_obj);
                // DestroyImmediate(root2);
                // Destroy(root2);
                Debugger.LogWarning(root);
                Debugger.LogWarning(ATest_Destroy_1.s_obj);
                break;

            case 1:
                // 切换场景时不删除
                DontDestroyOnLoad(root);
                break;

            default:
                // 切换场景时删除
                break;
        }

        // yield return new WaitForSeconds(1);

        // 只能load buildsetting中添加的scene 或 AssetBundle中的
        // SceneManager.LoadScene("UGUI_Test");

        yield return null;
    }
}