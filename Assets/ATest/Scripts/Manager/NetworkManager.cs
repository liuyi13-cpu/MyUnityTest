using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkManager : Base
{
    SocketClient m_socket;
    static Queue<KeyValuePair<int, ByteBuffer>> sEvents = new Queue<KeyValuePair<int, ByteBuffer>>();

    ///------------------------------------------------------------------------------------
    public override void StartEx()
    {
        m_socket = new SocketClient();
        m_socket.OnRegister();
    }

    public override void UpdateEx()
    {
        if (sEvents.Count > 0)
        {
            while (sEvents.Count > 0)
            {
                KeyValuePair<int, ByteBuffer> _event = sEvents.Dequeue();
                switch (_event.Key)
                {
                    case Protocal.Connect:
                        // TEST
                        SendCS_AccountLogin();
                        break;

                    case Protocal.Exception:
                        break;

                    case Protocal.Disconnect:
                        break;

                    default:
                        break;
                }
                // TODO
                // facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);
            }
        }
    }

    public override void OnDestroyEx()
    {
        m_socket.OnRemove();
    }

    ///------------------------------------------------------------------------------------
    public static void AddEvent(int _event, ByteBuffer data)
    {
        sEvents.Enqueue(new KeyValuePair<int, ByteBuffer>(_event, data));
    }


    /// <summary>
    /// 发送链接请求
    /// </summary>
    public void SendConnect()
    {
        m_socket.SendConnect();
    }

    /// <summary>
    /// 发送SOCKET消息
    /// </summary>
//     public void _sendMessage(MessageId msgID, IExtensible cmd)
//     {
//         MemoryStream stream = new MemoryStream();
//        stream.Position = 0;
//        Serializer.Serialize(stream, cmd);
//        byte[] buffer = stream.ToArray();
// 
//         m_socket.WriteMessage((int)msgID, buffer);
//     }
// 
    ///------------------------------------------------------------------------------------

    public void SendCS_AccountLogin()
    {
        Debug.Log("SendMessageTest");

//         CS_AccountLogin cmd = new CS_AccountLogin();
//         cmd.account = "wangxueping";
//         cmd.password = "123456";
// 
//         _sendMessage(MessageId.MICS_AccountLogin, cmd);
    }
}