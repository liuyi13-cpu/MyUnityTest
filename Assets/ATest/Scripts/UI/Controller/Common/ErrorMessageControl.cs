using com.fanxing.consts;
using UnityEngine;

public class ErrorMessageControl : BaseControllerNG
{
    public ErrorMessageControl(Transform parent)
    {
        Init(AssetPaths.ErrorMessage, typeof(ErrorMessageView), parent);
    }

    public void setText(string text)
    {
        GetView<ErrorMessageView>().setText(text);
    }
}
