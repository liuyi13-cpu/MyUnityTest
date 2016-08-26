using UnityEngine;
using System.Collections;

public class ATest_Destroy_1 : MonoBehaviour {

    static public GameObject s_obj;

    // Use this for initialization
    void Start () {
        StartCoroutine(TestDestroy());
    }

    IEnumerator TestDestroy()
    {
        yield return new WaitForSeconds(3);
        Debugger.LogWarning(s_obj);
    }
}
