using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class TunnelMessenger : IKNetworkMessenger
{
    public TcpClient TCP;
    public UdpClient UDP;

    private Action<KNetworkMessage> globalMsgCallback;
    private Action<KNetworkObjectMessage> objectMsgCallback;
    public void Init()
    {
        TCP = new TcpClient();
        UDP = new UdpClient();
        TCP.Connect(IPAddress.Parse(DebugMenu.IP), 24726);
        UDP.Connect(IPAddress.Parse(DebugMenu.IP), 24726);
        new Thread(() =>
        {
            for (; ; )
            {
                var available = TCP.Available;
                if (available > 0)
                {

                    try
                    {
                        var bfr = new byte[available];
                        TCP.Client.Receive(bfr);
                        var stream = new MemoryStream(bfr);
                        var reader = new BinaryReader(stream);
                        var typeName = reader.ReadString();
                        var type = Type.GetType(typeName);
                        var message = Activator.CreateInstance(type) as KNetworkMessage;
                        if (message is KNetworkObjectMessage objMsg)
                        {
                            objMsg.objectId = new KNetworkId(reader.ReadUInt64());
                            message.Deserialize(reader);
                            Dispatcher.RunOnMainThread(() => objectMsgCallback(objMsg));


                        }
                        else
                        {
                            message.Deserialize(reader);
                            Dispatcher.RunOnMainThread(() => globalMsgCallback(message));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    


                }
            }
        }).Start();
    }

    public void RegisterGlobalMessageCallback(Action<KNetworkMessage> callback)
    {
        globalMsgCallback = callback;
    }

    public void RegisterObjectMessageCallback(Action<KNetworkObjectMessage> callback)
    {
        objectMsgCallback = callback;
    }

    public void SendGlobalMessage(KNetworkMessage message, bool reliable = true)
    {
        //globalMsgCallback(message);
        var stream = new MemoryStream();
        var writer = new BinaryWriter(stream);
        writer.Write(message.GetType().AssemblyQualifiedName);
        message.Serialize(writer);
        writer.Flush();
        TCP.Client.Send(stream.ToArray());
        

    }
    public void SendUdpTest()
    {
        UDP.Client.Send(Encoding.ASCII.GetBytes("test"));
    }

    public void SendObjectMessage(KNetworkObjectMessage message, bool reliable = true)
    {
        var stream = new MemoryStream();
        var writer = new BinaryWriter(stream);
        writer.Write(message.GetType().AssemblyQualifiedName);
        writer.Write(message.objectId.uid);
        message.Serialize(writer);
        writer.Flush();
        TCP.Client.Send(stream.ToArray());
    }
}
