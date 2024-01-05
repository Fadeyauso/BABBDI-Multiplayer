using GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.tvOS;

public class BadassServer : MonoBehaviour
{
    public TcpListener TCP;
    public UdpClient UDP;

    public List<TcpClient> tcpClients = new List<TcpClient>();
    public List<UdpClient> udpClients = new List<UdpClient>();

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
                    tcpClients.Add(newClient);
                    Dispatcher.RunOnMainThread(() =>
                    {

                        KNetworkManager.instance.messenger.SendGlobalMessage(new OnPlayerConnectedMessage() { newPlayerId = tcpClients.Count - 1 });
                    });
                }
                foreach (var client in tcpClients)
                {
                    var available = client.Available;
                    if (available > 0)
                    {
                        byte[] data = new byte[available];
                        client.Client.Receive(data);
                        foreach (var subClient in tcpClients)
                        {
                            //if(subClient!=client)
                            {
                                subClient.Client.Send(data);
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
                    Debug.Log("UDP: " + Encoding.ASCII.GetString(buffer));
                    UDP.Client.SendTo(buffer, remoteEP);
                }
            }
        });
        udpThread.Start();
    }
}
