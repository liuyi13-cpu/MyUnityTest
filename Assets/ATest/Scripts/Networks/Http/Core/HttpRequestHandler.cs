using com.fanxing.protos;
using System;

namespace Networks.Http
{
    /// <summary>
    /// 客户端请求体
    /// </summary>
    public class HttpRequestHandler
    {
        public string Url;
        /// <summary>
        /// proto消息的id.
        /// </summary>
        public MessageId messageId;
        /// <summary>
        /// 请求的物件的类型
        /// </summary>
        public object messageObj;
        /// <summary>
        /// 服务器返回的 物件的类型
        /// </summary>
        public Type respObjType;
        /// <summary>
        /// 服务器返回时的回调.
        /// </summary>
        public Action<object> callback;
        /// <summary>
        /// 失败后的回调.
        /// </summary>
        public Action<Exception> failedCallback;

        public void Clear()
        {
            Url = null;
            messageId = 0;
            messageObj = null;
            respObjType = null;
            callback = null;
            failedCallback = null;
        }
    }
}
