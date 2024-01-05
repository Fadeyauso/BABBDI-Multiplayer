using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class KNetworkManager : MonoBehaviour
{
    public static bool killswitch = false;
    public const int PLAYER_ID_START = 10000;
    public static KNetworkManager instance;

    public IKNetworkMessenger messenger;

    private ulong lastAllocatedId;
    public Dictionary<KNetworkId,KNetworkObject> networkObjects = new Dictionary<KNetworkId, KNetworkObject>();



    public GameObject networkPlayerPrefab;

    public int localPlayerId = -1;
    public int numberOfPlayers;
    public void Awake()
    {
        instance = this; 
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnApplicationQuit()
    {
        killswitch = true;
    }
    public void Init()
    {
        localPlayerId = -1;
        messenger = new TunnelMessenger();
        messenger.Init();
        messenger.RegisterGlobalMessageCallback(OnGlobalMessageReceived);
        messenger.RegisterObjectMessageCallback(OnObjectMessageReceived);
    }


    void OnGlobalMessageReceived(KNetworkMessage message)
    {
        if(message is OnPlayerConnectedMessage onPlayerConnected)
        {
            var newPlayerId = onPlayerConnected.newPlayerId;
            Debug.Log($"Connected player with id {newPlayerId}. Your local id is {localPlayerId}");
            if(localPlayerId == -1)
            {
                localPlayerId = newPlayerId;
                Debug.Log("Found a local player");
                KNetworkManager.instance.messenger.SendGlobalMessage(new PlayerGreetingMessage() { playerId = KNetworkManager.instance.localPlayerId });

            }
            if (localPlayerId!=newPlayerId)
            {
                Debug.Log("Spawning a character for new player");

                var player = Instantiate(networkPlayerPrefab).GetComponent<KRemoteNetworkPlayer>();
                player.playerId = newPlayerId;
                var netPlr = player.GetNetObject();
                netPlr.objectId = new KNetworkId((ulong)(newPlayerId + PLAYER_ID_START));
                //RegisterNetworkObject(netPlr);
            }


        }
        if(message is PlayerGreetingMessage playerGreetingMessage)
        {
            if (playerGreetingMessage.playerId == localPlayerId) return;

            Debug.Log("New player has connected and is ready to receive messages. Id: " + playerGreetingMessage.playerId);
            messenger.SendGlobalMessage(new NotifyNewPlayerMessage() { playerId = localPlayerId });

        }
        if (message is NotifyNewPlayerMessage notifyNewPlayerMessage)
        {
            if (notifyNewPlayerMessage.playerId == localPlayerId) return;
            if (networkObjects.ContainsKey(new KNetworkId((ulong)(notifyNewPlayerMessage.playerId + PLAYER_ID_START)))) return;
            Debug.Log($"Got a notification from old player. Remote id: {notifyNewPlayerMessage.playerId}. Local id: {localPlayerId}");

            var player = Instantiate(networkPlayerPrefab).GetComponent<KRemoteNetworkPlayer>();
            player.playerId = notifyNewPlayerMessage.playerId;
            var netPlr = player.GetNetObject();
            netPlr.objectId = new KNetworkId((ulong)(notifyNewPlayerMessage.playerId + PLAYER_ID_START));
        }
        if(message is PlayerPositionSyncMessage onPlayerPositionSync)
        {
            if (onPlayerPositionSync.playerId == localPlayerId) return;
            var netId = new KNetworkId((ulong)(PLAYER_ID_START + onPlayerPositionSync.playerId));
            if (!networkObjects.ContainsKey(netId)) return;
            var plrGameObject = networkObjects[netId].gameObject;
            plrGameObject.transform.position = onPlayerPositionSync.position;
            plrGameObject.transform.eulerAngles = onPlayerPositionSync.rotation;
        }
        if(message is ItemPickedUpMessage itemPickedUpMessage)
        {
            if (itemPickedUpMessage.playerId == localPlayerId) return;
            var playerObject = networkObjects[new KNetworkId((ulong)(itemPickedUpMessage.playerId + PLAYER_ID_START))];
            var itemObject = networkObjects[new KNetworkId(itemPickedUpMessage.objectId)];
            var rb = itemObject.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.GetComponent<Collider>().isTrigger = true;
            itemObject.gameObject.transform.parent = playerObject.transform.Find("ObjectPos");
            itemObject.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            itemObject.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);

        }
        if (message is ItemDroppedMessage itemDroppedMessage)
        {
            if (itemDroppedMessage.playerId == localPlayerId) return;
            var playerObject = networkObjects[new KNetworkId((ulong)(itemDroppedMessage.playerId + PLAYER_ID_START))];
            var itemObject = networkObjects[new KNetworkId(itemDroppedMessage.objectId)];
            itemObject.gameObject.transform.parent = null;
            var rb = itemObject.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.detectCollisions = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.GetComponent<Collider>().isTrigger = false;
            rb.AddForce(playerObject.gameObject.transform.forward * 10f, ForceMode.Impulse);

        }

    }
    void OnObjectMessageReceived(KNetworkObjectMessage message)
    {
        var obj = networkObjects[message.objectId];
        var components = obj.gameObject.GetComponents<Component>();
        foreach (var comp in components)
        {
            var methods = comp.GetType().GetMethods();
            foreach (var met in methods)
            {
                var attr = met.GetCustomAttribute<KNetMessageHandlerAttribute>();

                if (attr != null)
                {
                    if (attr.messageType==message.GetType())
                    {
                        met.Invoke(comp, new object[] { message});
                    }    
                }
            }
        }
    }


    public void RegisterNetworkObject(KNetworkObject netObj)
    {
        Debug.Log("registering net object with id " + netObj.objectId.uid);

        networkObjects.Add(netObj.objectId,netObj);
    }
    public KNetworkId GetFreeNetworkId()
    {
        return new KNetworkId(++lastAllocatedId);
    }
    

}
public static class KNetworkUtils
{
    public static KNetworkObject GetNetObject(this MonoBehaviour mbh)
    {
        var component = mbh.gameObject.GetComponent<KNetworkObject>();
        return component;
    }
    public static bool IsANetworkObject(this MonoBehaviour mbh)
    {
        var component = mbh.gameObject.GetComponent<KNetworkObject>();
        return component != null;
    }
    public static KNetworkObject SendKNetMessage(this MonoBehaviour mbh, KNetworkObjectMessage message)
    {
        var component = mbh.gameObject.GetComponent<KNetworkObject>();
        message.objectId = component.objectId;
        KNetworkManager.instance.messenger.SendObjectMessage(message);
        return component;
    }
}
public class KNetMessageHandlerAttribute:Attribute
{
    public Type messageType;
    public KNetMessageHandlerAttribute(Type messageType)
    {
        this.messageType = messageType;
    }   
}
