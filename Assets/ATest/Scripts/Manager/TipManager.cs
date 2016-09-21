
using UnityEngine;

/// <summary>
/// 简单版本提示
/// </summary>
public class TipManager : Base
{
    public enum ShowType
    {
        Simple,     // 简单提示
        MessageBox  // 提示框
    }

    const int ERROR_MESSAGE_TIME = 5; // SECOND

    ErrorMessageControl _control;

    float _StartTime = 0;

    bool _ShowTip = false;

    public TipManager(Transform parent) : base(parent)
    {
    }

    public override void StartEx()
    {
        _control = new ErrorMessageControl(m_Root);
        _control.DoOpen();
        _control.SetActive(false);
    }

    public override void OnDestroyEx()
    {
        _control.DoClose();
        _control = null;
    }

    public override void FixedUpdateEx()
    {
        if (!_ShowTip) return;

        if (Time.realtimeSinceStartup - _StartTime > ERROR_MESSAGE_TIME)
        {
            _control.SetActive(false);
            _ShowTip = false;
        }
    }

    public void SetText(string text)
    {
        _control.SetActive(true);
        _control.setText(text);
        _StartTime = Time.realtimeSinceStartup;
        _ShowTip = true;
    }
}
