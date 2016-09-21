using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPoolManager : MonoBehaviour {
    //对象池物体的根
    GameObject _poolGameObjcetRoot;
    //对象池物体的字典
    Dictionary<string, GameObjectPool> _poolGameObjectDic = new Dictionary<string, GameObjectPool>();
    //从对象池中取出的物体
    //感觉遍历对象池字典更耗时，所以用个容器装起来
    Dictionary<GameObject, string> _poolOutGameObjectDic = new Dictionary<GameObject, string>();
    ////东西不够，临时创建的
    //List<GameObject> _createdGameObjectList = new List<GameObject>();

    int _initEndCount = 0;
    System.Action _endCallback = null;

    void Start()
    {
        if (_poolGameObjcetRoot == null)
        {
            _poolGameObjcetRoot = gameObject;
        }
        if (Project.gameOjbectPool == null)
        {
            Project.gameOjbectPool = this;
        }

    }
    public void SetGameObjectInfo(string resName, int maxCount)
    {
        if (_poolGameObjectDic.ContainsKey(resName))
        {
            _poolGameObjectDic[resName].ResetGameObjectPool();
        }
        else
        {
            _poolGameObjectDic.Add(resName, new GameObjectPool(_poolGameObjcetRoot, this));
        }
        _poolGameObjectDic[resName].SetGameObjectInfo(resName, maxCount);
    }
    /// <summary>
    /// 后边调整回掉为返回百分比，就可以兼容进度条的功能
    /// </summary>
    /// <param name="isSync"></param>
    /// <param name="endCallback"></param>
    public IEnumerator StartInit(bool isSync = false, System.Action endCallback = null)
    {
        if (isSync == true)
        {
            yield return new WaitForSeconds(0.5f);//无用代码，为了效果打的补丁
            _initEndCount = 0;
            _endCallback = endCallback;
            foreach (var item in _poolGameObjectDic)
            {
                item.Value.StartInit(isSync, SyncInitCallback);
                yield return null;
            }
        }
        else
        {
            foreach (var item in _poolGameObjectDic)
            {
                item.Value.StartInit(isSync);
            }
        }
    }
    public void SyncInitCallback()
    {
        _initEndCount++;
        if (_initEndCount >= _poolGameObjectDic.Count)
        {
            if (_endCallback != null)
            {
                _endCallback();
            }
        }
    }
    public GameObject GetGameObject(string resName)
    {
        GameObject go = null;
        if (!_poolGameObjectDic.ContainsKey(resName))
        {
            ////生成物体
           // GameObject goTemp = Resources.Load(resName) as GameObject;
           // go = GameObject.Instantiate(goTemp);
           // _poolOutGameObjectDic.Add(go, resName);
            Debugger.Log(string.Format("Resouce name:{0} is not manager by Pool.", resName));
        }
        else
        {
            //if(_poolGameObjectDic[resName].IsCanGetFreeGameObject())
            //{
            go = _poolGameObjectDic[resName].GetFreeGameObject();
            _poolOutGameObjectDic.Add(go, resName);
            //}
            //else//准备移动到pool里面，而不是放到manager中。 
            //{
            //    GameObject goTemp = Resources.Load(resName) as GameObject;
            //    go = GameObject.Instantiate(goTemp);
            //    _createdGameObjectList.Add(go);
            //}
        }        
        return go;
    }
    public void ReturnGameObject(GameObject go)
    {
        //if (_createdGameObjectList.Contains(go))
        //{
        //    _createdGameObjectList.Remove(go);
        //    Destroy(go);
        //    return;
        //}
        if (_poolOutGameObjectDic.ContainsKey(go))
        {
            _poolGameObjectDic[_poolOutGameObjectDic[go]].ReturnGameObject(go);
            _poolOutGameObjectDic.Remove(go);
            return;
        }
     //   Debugger.Log(string.Format("GameObject name : {0} is not create by pool.", go));
    }
    public bool IsContainsResouces(string resName)
    {
        return _poolGameObjectDic.ContainsKey(resName);
    }


}
