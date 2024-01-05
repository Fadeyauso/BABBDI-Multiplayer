using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDroppedMessage : KNetworkMessage
{
    public int playerId;
    public ulong objectId;
    public override void Serialize(BinaryWriter stream)
    {
        base.Serialize(stream);
        stream.Write(playerId);
        stream.Write(objectId);
    }
    public override void Deserialize(BinaryReader stream)
    {
        base.Deserialize(stream);
        playerId = stream.ReadInt32();
        objectId = stream.ReadUInt64();
    }
}