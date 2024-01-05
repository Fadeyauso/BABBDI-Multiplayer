using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KNetworkObject:MonoBehaviour
{
    public KNetworkId objectId;
    void Start()
    {
        if(objectId.uid == 0)
        {
            objectId = KNetworkManager.instance.GetFreeNetworkId();
        }
        KNetworkManager.instance.RegisterNetworkObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
