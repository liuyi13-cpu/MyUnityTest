using UnityEngine;

public class ATest_Socket : MonoBehaviour
{
    void Start()
    {
        // 链接服务器
        Main.Instance.m_NetMgr.SendConnect();
    }
}
