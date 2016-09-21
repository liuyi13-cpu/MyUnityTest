
#define DEBUG_NET

using com.fanxing.protos;
using Networks.Http;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : Base
{
    /// <summary>
    /// 超时时间
    /// </summary>
    const int TIMEOUT_TIME = 10;
    /// <summary>
    /// 重复次数
    /// </summary>
    const int RESEND_TIME = 10;

    int _resendTime = 0;
    public bool Pause { set; get; }

    HttpNetwork _httpNet = new HttpNetwork();

    /// <summary>
    /// 发送消息队列
    /// </summary>
    Queue<HttpRequestHandler> _requestQueue = new Queue<HttpRequestHandler>();
    /// <summary>
    /// 接收消息队列
    /// </summary>
    Queue<HttpResponseHandler> _responseQuene = new Queue<HttpResponseHandler>();
    /// <summary>
    /// 登录服Url
    /// </summary>
    public string UrlLogin { get; set; }
    /// <summary>
    /// 游戏服Url
    /// </summary>
    public string UrlGame { get; set; }
    /// <summary>
    /// 是否调用了断网接口
    /// </summary>
    bool IsNetBreak { set; get; }

    /// <summary>
    /// 消息回调接口
    /// </summary>
    readonly INetManager _Callback;

    public NetManager(INetManager netManagerControl)
    {
        _Callback = netManagerControl;
    }

    public override void StartEx()
    {
#if DEBUG_NET
        // 初始化收发消息池
        Main.Instance.m_PoolMgr.CreatePool<HttpRequestHandler>(_OnPoolGetElementRequest, _OnPoolPushElementRequest);
        Main.Instance.m_PoolMgr.CreatePool<HttpResponseHandler>(_OnPoolGetElementResponse, _OnPoolPushElementResponse);
#endif
    }

    public override void OnDestroyEx()
    {
        if (_httpNet != null)
        {
            _httpNet.Dispose();
            _httpNet = null;
        }
    }

    public void Reset()
    {
        _requestQueue.Clear();
        _responseQuene.Clear();
        _httpNet.Clear();
        Pause = false;
        IsNetBreak = false;
        _resendTime = 0;
    }

    public override void FixedUpdateEx()
    {
        if (Pause) return;

        _CheckRequest();
        _CheckResponse();
    }

    void _CheckRequest()
    {
        if (!_httpNet.IsBusy && _requestQueue.Count > 0)
        {
            _SendMsg(_requestQueue.Dequeue());
        }
    }

    void _CheckResponse()
    {
        //------ 1 collect target item.
        var arr = _httpNet.ResponseListToArray();
        if (arr.Count <= 0) return;

#if DEBUG_NET
        if (arr.Count > 1)
        {
            // Debugger.LogError("_CheckResponse COUNT > 1");
        }
#endif
        var enumerator = arr.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            if (item.respObj != null || item.error != null)
            {
                _responseQuene.Enqueue(item);
            }
            else
            {
                if (Time.realtimeSinceStartup - item.startTime > TIMEOUT_TIME)
                {
                    // TODO 超时
                    Debugger.LogWarning("time out,cancel.");
                    // item.CancelAsync();
                }
            }
        }

        //--------- 2 invoke callbacks
        var enumerator1 = _responseQuene.GetEnumerator();
        while (enumerator1.MoveNext())
        {
            var item = enumerator1.Current;
            if (item.error != null)
            {
                if (!IsNetBreak)
                {
                    IsNetBreak = true;
                    _Callback.OnNetBreak();

                    if (item.reqHandler.failedCallback != null)
                        item.reqHandler.failedCallback(item.error);
                }

                _resendTime++;
                if (_resendTime >= RESEND_TIME)
                {
                    // 限制重复次数
                    _resendTime = 0;
                    Pause = true;
                    _Callback.OnPause();
                }

                // 重新塞回发送队列-重新发送
                _requestQueue.Enqueue(item.reqHandler);

                _httpNet.RemoveResponeItem(item);
                // 释放池
                Main.Instance.m_PoolMgr.Release(item);
                item.Clear();
            }
            else
            {
                if (IsNetBreak)
                {
                    IsNetBreak = false;
                    _Callback.OnNetResume();
                    _resendTime = 0;
                }

                bool hasProcess = false;
                if (item.respObj is VO_Error)
                {
                    var respError = item.respObj as VO_Error;

                    hasProcess = _Callback.OnNetError(respError);
                }

                if (!hasProcess && item.reqHandler.callback != null)
                {
                    try
                    {
                        item.reqHandler.callback(item.respObj);
                    }
                    catch (Exception e)
                    {
                        Debugger.LogError("Get: " + e.Message);
                        throw e;
                    }
                }

                /// TODO
                _Callback.OnNetReceive();

                _httpNet.RemoveResponeItem(item);

                // 释放池
                Main.Instance.m_PoolMgr.Release(item.reqHandler);
                Main.Instance.m_PoolMgr.Release(item);
                item.reqHandler.Clear();
                item.Clear();
            }
        }

        _responseQuene.Clear();
    }

    public void SendMsg2LoginServer(
        MessageId messageId,
        object messageObj,
        Type respObjType,
        Action<object> callback = null,
        Action<Exception> failedCallback = null)
    {
        _SendMsg(UrlLogin, messageId, messageObj, respObjType, callback, failedCallback);
    }

    public void SendMsg2GameServer(
        MessageId messageId,
        object messageObj,
        Type respObjType,
        Action<object> callback = null,
        Action<Exception> failedCallback = null,
        bool isNoviceGuide = false, 
        bool isJumpNoviceGuide = false)
    {
        if (isNoviceGuide)
        {
            // 新手教学需要特殊处理
            Action<object> oldcallback = callback;
            Type oldrespObjType = respObjType;
            messageObj = _Callback.OnNovice(messageId, messageObj, isJumpNoviceGuide);
            messageId = MessageId.MICS_NoviceGuide;
            respObjType = typeof(SC_NoviceGuide);
            callback = (respObj) =>
            {
#if DEBUG_NET
                Debugger.Log("isNoviceGuide-----------------------------respObj " + respObj);
#endif
                var resp = respObj as SC_NoviceGuide;
                if (resp != null)
                {
                    var innerObj = HttpNetwork.GetDeserializeData(resp.datas, oldrespObjType);
                    if (oldcallback != null)
                        oldcallback(innerObj);
                }
            };
        }
 
        _SendMsg(UrlGame, messageId, messageObj, respObjType, callback, failedCallback);
    }

    void _SendMsg(string Url, 
        MessageId messageId, 
        object messageObj, 
        Type respObjType,
        Action<object> callback, 
        Action<Exception> failedCallback)
    {
        HttpRequestHandler req = Main.Instance.m_PoolMgr.Get<HttpRequestHandler>();
        req.messageId = messageId;
        req.messageObj = messageObj;
        req.respObjType = respObjType;
        req.callback = callback;
        req.failedCallback = failedCallback;
        req.Url = Url;

        if (_httpNet.IsBusy)
        {
            _requestQueue.Enqueue(req);
        }
        else
        {
            _SendMsg(req);
        }
    }

    void _SendMsg(HttpRequestHandler req)
    {
        _Callback.OnNetSend();

        HttpResponseHandler response = Main.Instance.m_PoolMgr.Get<HttpResponseHandler>();
        response.reqHandler = req;
        response.startTime = Time.realtimeSinceStartup;

        _httpNet.UpdateDataAsync(response);
    }

#if DEBUG_NET
    void _OnPoolGetElementRequest(HttpRequestHandler obj)
    {
        Debugger.Log("POOL GET --sendMsg");
    }

    void _OnPoolPushElementRequest(HttpRequestHandler obj)
    {
        if (obj.respObjType != null)
        {
            Debugger.Log("POOL PUSH --sendMsg:" + obj.respObjType.Name);
        }
        else
        {
            Debugger.Log("POOL PUSH --sendMsg:");
        }
    }

    void _OnPoolGetElementResponse(HttpResponseHandler obj)
    {
        Debugger.Log("POOL GET --receiveMsg");
    }

    void _OnPoolPushElementResponse(HttpResponseHandler obj)
    {
        if (obj.respObj != null)
        {
            Debugger.Log("POOL PUSH --receiveMsg:" + obj.respObj.GetType().Name);
        }
        else
        {
            Debugger.Log("POOL PUSH --receiveMsg");
        }
    }
#endif
}
