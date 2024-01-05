using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPositionSyncMessage:KNetworkMessage
{
    public int playerId;
    public Vector3 position;
    public Vector3 rotation;
    public override void Serialize(BinaryWriter stream)
    {
        base.Serialize(stream);
        stream.Write(playerId);
        stream.Write(position.x);
        stream.Write(position.y);
        stream.Write(position.z);
        stream.Write(rotation.x);
        stream.Write(rotation.y);
        stream.Write(rotation.z);
    }
    public override void Deserialize(BinaryReader stream)
    {
        base.Deserialize(stream);
        playerId = stream.ReadInt32();
        var posX = stream.ReadSingle();
        var posY = stream.ReadSingle();
        var posZ = stream.ReadSingle();
        var rotX = stream.ReadSingle();
        var rotY = stream.ReadSingle();
        var rotZ = stream.ReadSingle();
        position = new Vector3(posX, posY, posZ);
        rotation = new Vector3(rotX, rotY, rotZ);
    }
}