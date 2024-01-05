using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

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
        UDP = new UdpClient();

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

                        KNetworkManager.instance.messenger.SendGlobalMessage(new OnPlayerConnectedMessage() { newPlayerId=tcpClients.Count-1});
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
                if(UDP.Available>0)
                {
                    var result = UDP.ReceiveAsync();
                    result.Wait();
                }
            }
        });
        thread.Start();
    }
}
