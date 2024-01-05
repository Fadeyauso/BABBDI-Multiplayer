using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct KNetworkId
{
    public ulong uid;

    public KNetworkId(ulong uid)
    {
        this.uid = uid;
    }
    public override string ToString()
    {
        return uid.ToString();
    }
}
