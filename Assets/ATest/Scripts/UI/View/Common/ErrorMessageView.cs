using UnityEngine.UI;

/// <summary>
/// 界面 - 错误提示
/// </summary>
public class ErrorMessageView : BaseView
{
    Text _text;

    void OnEnable()
    {
        _text = transform.FindChild("Text").GetComponent<Text>();
    }

    public void setText(string text)
    {
        _text.text = text;
    }
}
