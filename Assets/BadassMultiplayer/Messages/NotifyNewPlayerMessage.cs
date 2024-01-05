using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class NotifyNewPlayerMessage:KNetworkMessage
{
    public int playerId;
    public Vector3 position;
    public Vector3 rotation;
    public override void Serialize(BinaryWriter stream)
    {
        base.Serialize(stream);
        stream.Write(playerId);
    }
    public override void Deserialize(BinaryReader stream)
    {
        base.Deserialize(stream);
        playerId = stream.ReadInt32();
    }
}