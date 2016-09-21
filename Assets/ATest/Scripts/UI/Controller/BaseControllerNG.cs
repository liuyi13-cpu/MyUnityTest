using System;
using UnityEngine;

/// <summary>
/// 为 mvc,c层的基类
/// </summary>
public abstract class BaseControllerNG
{
    /// <summary>
    /// View层
    /// </summary>
    BaseView            _view;
    Type                _viewType;
    string              _prefabPath;
    Transform          _Root;

    /// <summary>
    /// 预设
    /// </summary>
    protected GameObject GoPrefab { get; set; }

    protected void Init(string path, Type type, Transform parent = null)
    {
        _prefabPath = path;
        _viewType = type;
        _Root = parent;
    }

    public virtual void DoOpen()
    {
        if (GoPrefab == null)
        {
            GoPrefab = ResManager.Instantiate(_prefabPath);
            if (_Root != null)
            {
                GoPrefab.transform.parent = _Root;
            }
            var view = GoPrefab.GetComponent(_viewType);
            if (view != null)
            {
                ResManager.DestroyImmediate(view);
                Debugger.LogError("DO NOT ADD VIEW COMPONET TO PREFAB! PLS CHECK PREFAB:" + _prefabPath);
            }
            _view = GoPrefab.AddComponent(_viewType) as BaseView;

            var viewNG = _view as BaseViewNG;
            if (viewNG != null)
            {
                viewNG.SetControl(this);
            }
        }
    }

    public virtual void DoClose()
    {
        if (GoPrefab)
        {
            _view = null;
            ResManager.Destroy(GoPrefab);
            GoPrefab = null;
        }
    }

    public virtual void SetActive(bool active)
    {
        if (GoPrefab)
        {
            GoPrefab.SetActive(active);
        }
    }

    protected T GetView<T>() where T : BaseView
    {
        return _view as T;
    }
}
