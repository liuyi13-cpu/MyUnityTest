
#define DEBUG_NET

using com.fanxing.protos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Networks.Http
{
    /// <summary>
    /// 功能: 通过webclient发送http请求,并缓存http服务器的响应到responseList中.
    /// </summary>
    public class HttpNetwork : IDisposable
    {
#region headers.
        // client headers.
        const string HeaderMessageId = "MessageId";
        const string ReqHeaderCookie = "Cookie";

        //server response's headers.
        const string RespHeaderCookie = "Set-Cookie";
        const string RespHeaderMessageId = "messageid";
        const string RespContentType = "Content-Type";
        #endregion
        const int MAX_PACKAGE_LENGTH = 65 * 1000;

        //用于序列化(反)
        static ProtoSerializer serializer = new ProtoSerializer();
        static Regex sessionPattern = new Regex(@"jsessionid=\w+;");
        static Regex removeSessionHeaderPattern = new Regex("jsessionid=");

        WebClient client = new WebClient();
        List<HttpResponseHandler> responseList = new List<HttpResponseHandler>();

        public HttpNetwork()
        {
            client.UploadDataCompleted += Client_UploadDataCompleted;
        }

        public static byte[] GetSerilizedBytes(object obj) {
            byte[] bytes = null;
            using (var ms = new MemoryStream()) {
                serializer.Serialize(ms, obj);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        public static object GetDeserializeData(byte[] data, Type type) {
            using (MemoryStream ms = new MemoryStream()) {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                object o = serializer.Deserialize(ms, null, type);
                return o;
            }
        }

        public static string GetSessionId(string cookie)
        {
            cookie = cookie.ToLower();
            var match = sessionPattern.Match(cookie);
            if (match.Success)
            {
                var nosession = removeSessionHeaderPattern.Replace(match.Value, "");
                if (!string.IsNullOrEmpty(nosession))
                {
                    return nosession.Substring(0, nosession.Length - 1);
                }
            }
            return null;
        }

        WebHeaderCollection GetHeader(MessageId messageId)
        {
            var headers = new WebHeaderCollection();
            headers[HeaderMessageId] = messageId.ToString();
            if (!string.IsNullOrEmpty(HttpCookie.currentCookie))
                headers[ReqHeaderCookie] = HttpCookie.currentCookie;
            return headers;
        }

        public void UpdateDataAsync(HttpResponseHandler handler)
        {
#if DEBUG_NET
            if (Debugger.useLog)
            {
                var sb = StringBuilderCache.Acquire();
                sb.AppendFormat("--> Request: url:{0},messageId:{1},value:{2},cookie:{3}",
                    handler.reqHandler.Url,
                    handler.reqHandler.messageId,
                    handler.reqHandler.messageObj,
                    HttpCookie.currentCookie);
                Debugger.Log(sb.ToString());
                StringBuilderCache.Release(sb);
            }
#endif

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, handler.reqHandler.messageObj);
                byte[] data = ms.ToArray();
                if (data.Length <= MAX_PACKAGE_LENGTH)
                {
                    handler.requestUri = new Uri(handler.reqHandler.Url);
                    handler.client = client;
                    responseList.Add(handler);

                    client.Headers = GetHeader(handler.reqHandler.messageId);
                    client.UploadDataAsync(handler.requestUri, null, data, handler);
                }
                else
                {
                    throw new Exception("Max package length more than 65k");
                }
            }
        }

        /// <summary>
        /// 发送一个http请求.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="requestObject"></param>
        /// <param name="handler"></param>
        public void UpdateDataAsync(string url, MessageId messageId, object requestObject, HttpResponseHandler handler)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, requestObject);
                byte[] data = ms.ToArray();
                if (data.Length <= MAX_PACKAGE_LENGTH)
                {
                    handler.respObj = null;
                    handler.requestUri = new Uri(url);
                    handler.client = client;
                    responseList.Add(handler);

                    client.Headers = GetHeader(messageId);
                    client.UploadDataAsync(handler.requestUri, null, data, handler);
                }
                else
                {
                    throw new Exception("Max package length more than 65k");
                }
            }
        }
        public bool IsBusy
        {
            get { return client.IsBusy; }
        }

        public void CancelAsync()
        {
            client.CancelAsync();
        }

        /// <summary>
        /// 解析ContentType,如: Content-Type:text/plain; messageid=MIVO_Error;charset=UTF-8
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        string GetMessageIdFromContentType(string contentType)
        {
            if (!string.IsNullOrEmpty(contentType))
            {
                string[] segs = contentType.Split(';');
                IEnumerable<string> q = segs.Select<string, string>(seg => seg.Trim());

                foreach (var item in q)
                {
                    string[] kv = item.Split('=');
                    if (kv[0].Trim() == RespHeaderMessageId)
                        return kv[1];
                }
            }
            return null;
        }

        void Client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                HttpResponseHandler handler = e.UserState as HttpResponseHandler;
                handler.error = e.Error;
                //we get errors, abort.
                if (e.Error != null)
                {
#if DEBUG_NET
                    if (Debugger.useLog)
                    {
                        var sb = StringBuilderCache.Acquire();
                        sb.AppendFormat("<---- Response Error: url:{0},error:{1},messageId:{2}",
                            handler.requestUri,
                            handler.error,
                            handler.reqHandler.messageId);
                        Debugger.Log(sb.ToString());
                        StringBuilderCache.Release(sb);
                    }
#endif
                }
                else
                {
                    // ok. get message correct.
                    ParseResponse(handler, e.Result);
                }
            }
        }

        void ParseResponse(HttpResponseHandler handler,byte[] respData)
        {
            //check response's headers.
            if (client.ResponseHeaders.HasKeys())
            {
                handler.messageId = GetMessageIdFromContentType(client.ResponseHeaders[RespContentType]);
                if (!string.IsNullOrEmpty(client.ResponseHeaders[RespHeaderCookie]))
                    HttpCookie.currentCookie = client.ResponseHeaders[RespHeaderCookie];
            }

            // check response's messageId,
            if (string.IsNullOrEmpty(handler.messageId))
            {
                handler.error = new Exception("response message is not protocol type. url:" + handler.requestUri);
            }
#if DEBUG_NET
            if (Debugger.useLog)
            {
                var sb = StringBuilderCache.Acquire();
                sb.AppendFormat("<-- Response: url:{0},error:{1},messageId:{2}", handler.requestUri, handler.error, handler.messageId);
                Debugger.Log(sb.ToString());
                StringBuilderCache.Release(sb);
            }
#endif
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(respData, 0, respData.Length);
                ms.Position = 0;
                //------ response error.
                if (handler.messageId == MessageId.MIVO_Error.ToString())
                {
                    handler.reqHandler.respObjType = typeof(VO_Error);
                }
                handler.respObj = serializer.Deserialize(ms, null, handler.reqHandler.respObjType);
            }
        }

        public void RemoveResponeItem(HttpResponseHandler item)
        {
            if (responseList.Contains(item))
                responseList.Remove(item);
        }

        public List<HttpResponseHandler> ResponseListToArray()
        {
            return responseList;//.ToArray();
        }

        public void Clear()
        {
            responseList.Clear();
        }

#region IDisposable Support
        private bool disposedValue = false; // 若要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 处置托管的状态(托管的对象)。
                    if (client != null)
                    {
                        responseList.Clear();
                        client.UploadDataCompleted -= Client_UploadDataCompleted;
                        client.Dispose();
                    }
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                client = null;
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~HttpNetwork() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing)中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing)中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
#endregion
    }
}
