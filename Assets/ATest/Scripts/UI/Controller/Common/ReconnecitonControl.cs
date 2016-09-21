using com.fanxing.consts;

public class ReconnecitonControl : BaseControllerNG
{
    public ReconnecitonControl()
    {
        Init(AssetPaths.ReconnectionCanvas, typeof(ReconnectionView));
    }

    public void ChangeStage(ReconnectionView.Stage stage)
    {
        GetView<ReconnectionView>().ChangeStage((int)stage);
    }
}
