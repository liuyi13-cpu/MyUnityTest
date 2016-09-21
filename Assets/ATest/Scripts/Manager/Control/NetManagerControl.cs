using com.fanxing.consts;
using com.fanxing.protos;
using com.fanxing.scene;
using Networks.Http;

public class NetManagerControl : INetManager
{
    ReconnecitonControl _ReconnecitonControl = new ReconnecitonControl();

    /// <summary>
    /// 错误处理
    /// </summary>
    public bool OnNetError(VO_Error voErr)
    {
        var err = voErr.errorCode;
        switch (err)
        {
            case DB_ERROR_MESSSAGE_CONST.DO_REPEAT_LOGIN:
                // 重登陆
                HttpCookie.Reset();
                Common.RepeatLoginNotice();
                return true;

            case DB_ERROR_MESSSAGE_CONST.ERROR_NO_TIMEOUT:
                // TODO
                break;

            default:
                // TODO
                Common.DealWithError(voErr);
                break;
        }

        return false;
    }

    /// <summary>
    /// 断网回调
    /// </summary>
    public void OnNetBreak()
    {
        MaskCanvasControl.CloseMaskCanvas();
        _ReconnecitonControl.DoOpen();
        EventSystemControl.GetInstance().SetEventSystemEnable(false);
    }

    /// <summary>
    /// 网络恢复回调
    /// </summary>
    public void OnNetResume()
    {
        _ReconnecitonControl.DoClose();
        EventSystemControl.GetInstance().SetEventSystemEnable(true);
    }

    /// <summary>
    /// 新手教学
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="messageObj"></param>
    /// <returns></returns>
    public object OnNovice(MessageId messageId, object messageObj, bool isJumpNoviceGuide)
    {
        var newMsg = new CS_NoviceGuide();

        newMsg.step = Common.curFlowId;
        newMsg.datas = HttpNetwork.GetSerilizedBytes(messageObj);
        if (messageId > 0)
            newMsg.messageId = (int)messageId;
        if (isJumpNoviceGuide)
        {
            newMsg.messageId = -1;
        }

        return newMsg;
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void OnNetSend()
    {
        // TODO
        // MaskCanvasControl.OpenMaskCanvas();
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    public void OnNetReceive()
    {
        // TODO
        // MaskCanvasControl.CloseMaskCanvas();
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    public void OnPause()
    {
        EventSystemControl.GetInstance().SetEventSystemEnable(true);
        _ReconnecitonControl.ChangeStage(ReconnectionView.Stage.Stage2);
    }
}
