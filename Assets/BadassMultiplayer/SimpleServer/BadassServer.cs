using GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class BadassServer : MonoBehaviour
{
    public TcpListener TCP;
    public UdpClient UDP;

    public Dictionary<int,TcpClient> tcpClients = new Dictionary<int, TcpClient>();
    public Dictionary<int, EndPoint> udpClients = new Dictionary<int, EndPoint>();

    private void OnApplicationQuit()
    {
        KNetworkManager.killswitch = true;
    }
    public void Init()
    {
        TCP = new TcpListener(IPAddress.Any, 24726);
        UDP = new UdpClient(new IPEndPoint(IPAddress.Any, 24726));

        TCP.Start();
        
        var thread = new Thread(() =>
        {
            for (; ; )
            {
                if (KNetworkManager.killswitch)
                    return;
                if (TCP.Pending())
                {
                    var newClient = TCP.AcceptTcpClient();
                    tcpClients.Add(tcpClients.Count,newClient);
                    Dispatcher.RunOnMainThread(() =>
                    {

                        KNetworkManager.instance.messenger.SendGlobalMessage(new OnPlayerConnectedMessage() { newPlayerId = tcpClients.Count });
                    });
                }
                foreach (var client in tcpClients)
                {
                    var available = client.Value.Available;
                    if (available > 0)
                    {
                        byte[] data = new byte[available];
                        client.Value.Client.Receive(data);
                        foreach (var subClient in tcpClients)
                        {
                            //if(subClient!=client)
                            {
                                subClient.Value.Client.Send(data);
                            }
                        }
                    }
                }
                

                
            }
        });
        thread.Start();
        var udpThread = new Thread(() =>
        {
            for(; ; )
            {
                IPEndPoint remoteEP;
                byte[] buffer;

                remoteEP = null;
                buffer = UDP.Receive(ref remoteEP);

                if (buffer != null && buffer.Length > 0)
                {
                    if (buffer[0] == 0x47 &&
                    buffer[1] == 0x41 &&
                    buffer[2] == 0x59)
                    {
                        Debug.Log("Registering new client with id "+(udpClients.Count));
                        udpClients.Add(udpClients.Count, remoteEP);
                    }
                    else
                    {
                        foreach (var subClient in udpClients)
                        {
                            UDP.Client.SendTo(buffer, subClient.Value);
                        }
                    }

                    //UDP.Client.SendTo(buffer, tcpClients[0].Client.RemoteEndPoint);
                    
                }
            }
        });
        udpThread.Start();
    }
}
