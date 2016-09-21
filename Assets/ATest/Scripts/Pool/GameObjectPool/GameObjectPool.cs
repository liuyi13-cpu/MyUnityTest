using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameObjectPool{
    //空闲物体
    List<GameObject> _freeGameObject;
    //正在使用的物体
    List<GameObject> _outputGameObject;
    List<GameObject> _createGameObject;
    GameObject _gameObjcetRoot;
    MonoBehaviour _mono;
    GameObject _resGameObject;

    string _resName;
    int _maxCount;
    Action _endCallback = null;
    public GameObjectPool(GameObject gameObjcetRoot, MonoBehaviour mono)
    {
        _gameObjcetRoot = gameObjcetRoot;
        _mono = mono;
    }
    public void SetGameObjectInfo(string resName, int maxCount)
    {
        _freeGameObject = new List<GameObject>(maxCount);
        _outputGameObject = new List<GameObject>(maxCount);
        _createGameObject = new List<GameObject>();
        _maxCount = maxCount;
        _resName = resName;
        //_resGameObject = Resources.Load<GameObject>(resName);
    }
    public void StartInit(bool isSync = false, Action endCallback = null)
    {
        if (isSync)
        {
            _endCallback = endCallback;
            _mono.StartCoroutine(SyncCreateGameObject());
        }
        else
        {
            CreateGameObject();
        }
    }
    IEnumerator SyncCreateGameObject()
    {
        _resGameObject = Resources.Load<GameObject>(_resName);
        yield return null;
        for (int i = 0; i < _maxCount; i++)
        {
            GameObject go = GameObject.Instantiate(_resGameObject);
            go.transform.SetParent(_gameObjcetRoot.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            _freeGameObject.Add(go);
            yield return null;
        }
        if (_endCallback != null)
        {
            _endCallback();
        }
    }
    void CreateGameObject()
    {
        _resGameObject = Resources.Load<GameObject>(_resName);
        for (int i = 0; i < _maxCount; i++)
        {
            GameObject go = GameObject.Instantiate(_resGameObject);
            go.transform.SetParent(_gameObjcetRoot.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            _freeGameObject.Add(go);
        }
    }
    public void ResetGameObjectPool()
    {
        foreach (var item in _freeGameObject)
        {
            GameObject.Destroy(item);
        }
        _freeGameObject = null;
        foreach (var item in _outputGameObject)
        {
            GameObject.Destroy(item);
        }
        foreach (var item in _createGameObject)
        {
            GameObject.Destroy(item);
        }
        _outputGameObject = null;
        _resGameObject = null;
        _resName = "";
        _maxCount = 0;

    }
    public bool IsCanGetFreeGameObject()
    {
        return _freeGameObject.Count > 0;
    }
    public GameObject GetFreeGameObject()
    {
        GameObject go = null;
        if (IsCanGetFreeGameObject())
        {
            go = _freeGameObject[0];
            _freeGameObject.RemoveAt(0);
            _outputGameObject.Add(go);
        }
        else
        {
            go = GameObject.Instantiate(_resGameObject);
            _createGameObject.Add(go);
        }
        //go.SetActive(true);
        return go;
    }
    public void ReturnGameObject(GameObject go)
    {
        if (IsGetFromPool(go))
        {
            _outputGameObject.Remove(go);
            _freeGameObject.Add(go);
            go.transform.SetParent(_gameObjcetRoot.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            //根据需求决定是否要隐藏
            //go.SetActive(false);
            return;
        }
        if (IsCreateFromPool(go))
        {
            //go.SetActive(false);
            _createGameObject.Remove(go);
            //先放回来再删掉，或者立即删掉。
            //go.transform.SetParent(_gameObjcetRoot.transform);
            GameObject.Destroy(go);
            return;
        }
    }
    public bool IsGetFromPool(GameObject go)
    {
        return _outputGameObject.Contains(go);
    }
    public bool IsCreateFromPool(GameObject go)
    {
        return _createGameObject.Contains(go);
    }
}
