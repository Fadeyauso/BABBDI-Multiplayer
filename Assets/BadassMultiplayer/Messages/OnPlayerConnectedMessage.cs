using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OnPlayerConnectedMessage : KNetworkMessage
{
    public int newPlayerId;
    public override void Serialize(BinaryWriter stream)
    {
        base.Serialize(stream);
        stream.Write(newPlayerId);
    }
    public override void Deserialize(BinaryReader stream)
    {
        base.Deserialize(stream);
        newPlayerId = stream.ReadInt32();
    }
}
