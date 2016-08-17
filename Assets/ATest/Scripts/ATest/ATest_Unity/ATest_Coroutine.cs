using UnityEngine;
using System.Collections;

/// <summary>
/// 1.StartCoroutine马上执行协程
/// 2.yield return 马上执行，然后pause返回，按条件resume
/// 3.Coroutine断点模式下，F10单步位置有问题，变量不显示
/// 4.多Coroutine相互不影响，同一帧里顺序执行。
/// 5.yield return null下一帧resume
/// </summary>
public class ATest_Coroutine : MonoBehaviour
{
    void Start()
    {
        print("Starting " + Time.time);
        StartCoroutine(WaitNullAndPrint());
        StartCoroutine(WaitAndPrint(2.0F));
        StartCoroutine(WaitAndPrint2(2.0F));
        print("Done " + Time.time);
    }

    IEnumerator WaitNullAndPrint()
    {
        print("WaitNullAndPrint start: " + Time.frameCount);
        yield return null;
        print("WaitNullAndPrint end : " + Time.frameCount);
    }

    IEnumerator WaitAndPrint(float waitTime)
    {
        print("WaitAndPrint start: " + Time.frameCount);
        yield return new WaitForSeconds(waitTime);
        print("WaitAndPrint end : " + Time.frameCount);
    }

    IEnumerator WaitAndPrint2(float waitTime)
    {
        print("WaitAndPrint2 start: " + Time.frameCount);
        yield return new WaitForSeconds(waitTime);
        print("WaitAndPrint2 end: " + Time.frameCount);
    }

    // Update is called once per frame
    void Update()
    {
       print("Update : " + Time.frameCount);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        print("LateUpdate : " + Time.frameCount);
    }
}
