using com.fanxing.protos;

public interface INetManager
{
    /// <summary>
    /// 错误处理
    /// </summary>
    bool OnNetError(VO_Error err);
    /// <summary>
    /// 断网回调
    /// </summary>
    void OnNetBreak();
    /// <summary>
    /// 网络恢复回调
    /// </summary>
    void OnNetResume();
    /// <summary>
    /// 新手教学
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="messageObj"></param>
    /// <returns></returns>
    object OnNovice(MessageId messageId, object messageObj, bool isJumpNoviceGuide);
    /// <summary>
    /// 发送消息
    /// </summary>
    void OnNetSend();
    /// <summary>
    /// 接收消息
    /// </summary>
    void OnNetReceive();
    /// <summary>
    /// 重连失败暂停界面
    /// </summary>
    void OnPause();
}