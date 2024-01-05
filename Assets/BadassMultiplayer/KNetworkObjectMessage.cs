using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KNetworkObjectMessage : KNetworkMessage
{
    public KNetworkId objectId;
    public KNetworkObjectMessage()
    {

    }
    public KNetworkObjectMessage(KNetworkObject obj)
    {
        objectId = obj.objectId;
    }
}
