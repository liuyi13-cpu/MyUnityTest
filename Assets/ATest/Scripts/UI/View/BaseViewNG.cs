using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 有多个Stage的View的基类
/// </summary>
public class BaseViewNG : BaseView
{
    List<GameObject> _StageList = new List<GameObject>();
    int _CurStage;

    /// <summary>
    /// 组件事件队列
    /// </summary>
    List<UnityEvent> _EventList = new List<UnityEvent>();

    BaseControllerNG _Control;
    public void SetControl(BaseControllerNG control)
    {
        _Control = control;
    }

    protected T GetControl<T>() where T : BaseControllerNG
    {
        return _Control as T;
    }

    void Start()
    {
        _CheckStage();
        ChangeStage(0); // default show stage 0

        StartEX();
    }

    void OnDestroy()
    {
        OnDestroyEX();

        var enumerator = _EventList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.RemoveAllListeners();
        }
        _EventList.Clear();
    }

    protected virtual void StartEX()
    {
    }

    protected virtual void OnDestroyEX()
    {
    }

    void _CheckStage()
    {
        // "Stage"不能被隐藏
        var stageRoot = GameObject.Find("Stage").transform;
        int childCount = stageRoot.childCount;
        if (stageRoot == null || childCount <= 0)
        {
            Debugger.LogError("INVALID STAGE, PLS CHECK UI PREFAB: " + gameObject.name);
            return;
        }

        for (int i = 0; i < childCount; i++)
        {
            _StageList.Add(stageRoot.GetChild(i).gameObject);
        }
    }

    protected void AddButtonListener(string name, UnityAction action)
    {
        Transform btnGO = null;
        var enumerator = _StageList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            btnGO = enumerator.Current.transform.FindChild(name);
            if (btnGO != null)
            {
                break;
            }
        }

        if (btnGO == null)
        {
            btnGO = GameObject.Find(name).transform;
            if (btnGO != null)
            {
                // 层级不对 Button需用放在StageX下， 避免使用GameObject.Find
                Debugger.LogError("层级不对 Button需用放在StageX下: " + gameObject.name + " /// " + name);
            }
        }

        if (btnGO == null)
        {
            Debugger.LogError("NOT FIND: " + gameObject.name + " /// " + name);
            return;
        }

        Button btn = btnGO.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(action);
        }

        _EventList.Add(btn.onClick);
    }

    /// <summary>
    /// 切换stage
    /// </summary>
    /// <param name="stage"></param>
    public void ChangeStage(int stage)
    {
        if (stage >= _StageList.Count)
        {
            Debugger.LogError("INVALID STAGE: " + stage);
            return;
        }

        _CurStage = stage;

        var enumerator = _StageList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.SetActive(false);
        }
        _StageList[_CurStage].SetActive(true);
    }
}