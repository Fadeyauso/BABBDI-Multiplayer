using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKNetworkMessenger
{
    void Init();
    void SendGlobalMessage(KNetworkMessage message, bool reliable = true);
    void SendObjectMessage(KNetworkObjectMessage message, bool reliable = true);
    void RegisterGlobalMessageCallback(Action<KNetworkMessage> callback);
    void RegisterObjectMessageCallback(Action<KNetworkObjectMessage> callback);
}
