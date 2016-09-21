using com.fanxing.consts;
using com.fanxing.scene;
using UnityEngine.UI;

/// <summary>
/// 界面 - 断网重连
/// </summary>
public class ReconnectionView : BaseViewNG
{
    Button _BtnYes;
    Button _BtnNo;

    protected override void StartEX()
    {
        AddButtonListener("ButtonYes", ButtonYes);
        AddButtonListener("ButtonNo", ButtonNo);
    }

    public enum Stage
    {
        Stage1,
        Stage2,
    }

    public void ButtonYes()
    {
        // 重连
        Main.Instance.m_NetMgr.Pause = false;

        EventSystemControl.GetInstance().SetEventSystemEnable(false);

        ChangeStage((int)Stage.Stage1);
    }

    public void ButtonNo()
    {
        Main.Instance.m_NetMgr.Reset();

        string curSceneName = SceneManagerNG.GetActiveScene().name;
        // 登录前回到第一个界面
        if (curSceneName.Equals(Scenes.versionChecking.ToString()))
        {
            Utils.Quit();
        }
        else if (curSceneName.Equals(Scenes.login.ToString()))
        {
            GetControl<ReconnecitonControl>().DoClose();
            // 登录界面
        }
        else
        {
            // 游戏里回到登录界面
            SceneManagerNG.LoadScene(Scenes.login.ToString());
        }
    }
}
