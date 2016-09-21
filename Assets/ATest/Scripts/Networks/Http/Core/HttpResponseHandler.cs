using System;
using System.Net;

namespace Networks.Http
{
    /// <summary>
    /// 服务器响应后的处理器.
    /// </summary>
    public class HttpResponseHandler
    {
        /// <summary>
        /// 服务器返回的响应对象
        /// </summary>
        public object respObj;
        public string messageId;
        public Uri requestUri;
        /// <summary>
        /// 返回的异常信息
        /// </summary>
        public Exception error;
        /// <summary>
        /// 客户端的请求
        /// </summary>
        public HttpRequestHandler reqHandler;
        /// <summary>
        /// 发送的时间.
        /// </summary>
        public float startTime;
        public WebClient client;

        public void CancelAsync()
        {
            if(client != null)
            {
                client.CancelAsync();
                error = new Exception("timeout!");
            }
        }

        public void Clear()
        {
            respObj = null;
            messageId = null;
            requestUri = null;
            error = null;
            reqHandler = null;
            startTime = 0f;
            client = null;
        }
    }
}
