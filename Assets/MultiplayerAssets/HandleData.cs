using System;
using System.IO;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BabbdiMultiplayer
{
    public static class HandleData
    {
        public static void HandleMessage(Packet _packet)
        {
            var msg = _packet.ReadString();
            Debug.Log(msg);
        }

        public static void HandleMovement(Packet _packet)
        {
            BABBDIPlayer.Instance.BABBDIPlayerMovement.ReceivedPosition = _packet.ReadVector3();
            BABBDIPlayer.Instance.BABBDIPlayerMovement.ReceivedRotation = _packet.ReadFloat();
        }
        public static void HandleDropItem(Packet _packet)
        {
            var id = _packet.ReadInt();
            var pos = _packet.ReadVector3();
            var forward = _packet.ReadVector3();
            var pickupItem = GameObject.FindObjectOfType<PickupItem>();
            GameObject go = null;
            if (id == 0)
            {
                go = Object.Instantiate<GameObject>(pickupItem.soap, pos, Quaternion.identity);//.inHands = true;
            }
            if (id == 1)
            {
                go = Object.Instantiate<GameObject>(pickupItem.club, pos, Quaternion.identity);
            }
            if (id == 2)
            {
                go = Object.Instantiate<GameObject>(pickupItem.climber, pos, Quaternion.identity);
            }
            if (id == 4)
            {
                go = Object.Instantiate<GameObject>(pickupItem.flashlight, pos, Quaternion.identity);
            }
            if (id == 5)
            {
                go = Object.Instantiate<GameObject>(pickupItem.propeller, pos, Quaternion.identity);
            }
            if (id == 6)
            {
                go = Object.Instantiate<GameObject>(pickupItem.blower, pos, Quaternion.identity);
            }
            if (id == 7)
            {
                go = Object.Instantiate<GameObject>(pickupItem.ball, pos, Quaternion.identity);
            }
            if (id == 8)
            {
                go = Object.Instantiate<GameObject>(pickupItem.bigball, pos, Quaternion.identity);
            }
            if (id == 9)
            {
                go = Object.Instantiate<GameObject>(pickupItem.stick, pos, Quaternion.identity);
            }
            if (id == 10)
            {
                go = Object.Instantiate<GameObject>(pickupItem.grabber, pos, Quaternion.identity);
            }
            if (id == 11)
            {
                go = Object.Instantiate<GameObject>(pickupItem.motorBike, pos, Quaternion.identity);
            }
            if (id == 12)
            {
                go = Object.Instantiate<GameObject>(pickupItem.compass, pos, Quaternion.identity);
            }
            if (id == 13)
            {
                 go = Object.Instantiate<GameObject>(pickupItem.trumpet, pos, Quaternion.identity);
            }
            if (id == 16)
            {
                go = Object.Instantiate<GameObject>(pickupItem.secretfinder, pos, Quaternion.identity);
            }
            go.transform.SetParent(null);
            var rb = go.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(forward * 10f, ForceMode.Impulse);
            rb.useGravity = true;
            rb.GetComponent<Collider>().isTrigger = false;
            var itemContainer = BABBDIPlayer.Instance.transform.Find("itemContainer");
            foreach(Transform t in itemContainer.transform)
            {
                GameObject.Destroy(t.gameObject);
            }
        }
        public static void HandlePickup(Packet _packet)
        {
            Debug.Log("Dolboeb");
            var pickupItem = GameObject.FindObjectOfType<PickupItem>();
            var spawnPos = _packet.ReadVector3();
            var id = _packet.ReadInt();
            Debug.Log("Picked up item: " + id);

            if (id == 0)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.soap, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer")).GetComponent<InteractObject>();//.inHands = true;
            }
            if (id == 1)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.club, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());                //gameObject.GetComponent<InteractObject>().inHands = true;
                //GameObject.Find("Player").GetComponent<FirstPersonController>().club = gameObject;
            }
            if (id == 2)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.climber, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
                //GameObject.Find("Player").GetComponent<FirstPersonController>().climber = gameObject2;
            }
            if (id == 4)
            {
                var go =  Object.Instantiate<GameObject>(pickupItem.flashlight, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer")).GetComponent<InteractObject>();//.inHands = true;
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
            }
            if (id == 5)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.propeller, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());                //gameObject3.GetComponent<InteractObject>().inHands = true;
                //GameObject.Find("Player").GetComponent<FirstPersonController>().propeller = gameObject3;
            }
            if (id == 6)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.blower, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
                //gameObject4.GetComponent<InteractObject>().inHands = true;
                //GameObject.Find("Player").GetComponent<FirstPersonController>().blower = gameObject4;
            }
            if (id == 7)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.ball, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer")).GetComponent<InteractObject>();//.inHands = true;
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
            }
            if (id == 8)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.bigball, spawnPos , Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer")).GetComponent<InteractObject>();//.inHands = true;
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
            }
            if (id == 9)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.stick, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer")).GetComponent<InteractObject>();//.inHands = true;
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
            }
            if (id == 10)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.grabber, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
                //gameObject5.GetComponent<InteractObject>().inHands = true;
                //GameObject.Find("Player").GetComponent<FirstPersonController>().grabber = gameObject5;
            }
            if (id == 11)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.motorBike, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
                //gameObject6.GetComponent<InteractObject>().inHands = true;
                //GameObject.Find("Player").GetComponent<FirstPersonController>().motorBike = gameObject6;
            }
            if (id == 12)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.compass, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));//.GetComponent<InteractObject>().inHands = true;
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
            }
            if (id == 13)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.trumpet, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));//.GetComponent<InteractObject>().inHands = true;
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
            }
            if (id == 16)
            {
                var go = Object.Instantiate<GameObject>(pickupItem.secretfinder, spawnPos, Quaternion.identity, BABBDIPlayer.Instance.transform.Find("itemContainer"));//.GetComponent<InteractObject>().inHands = true;
                GameObject.Destroy(go.GetComponent<InteractObject>());
                GameObject.Destroy(go.GetComponent<Rigidbody>());
            }
        }

        public static void HandleAnimations(Packet _packet)
        {
        }

        public static void HandleCameraAngle(Packet _packet)
        {
            //Beatrix.Instance.BeatrixVacpack.ReceivedCameraAngle = _packet.ReadFloat();
        }

        public static void HandleVacconeState(Packet _packet)
        {
            //Beatrix.Instance.BeatrixVacpack.VacMode = _packet.ReadBool();
        }

        public static void HandleGameModeSwitch(Packet _packet)
        {
            Statics.FriendInGame = _packet.ReadBool();
        }

        public static void HandleTime(Packet _packet)
        {
            //var time = _packet.ReadDouble();

            /*
            if (SRSingleton<SceneContext>.Instance != null)
            {
                if (SRSingleton<SceneContext>.Instance.TimeDirector != null)
                {
                    SRSingleton<SceneContext>.Instance.TimeDirector._worldModel.worldTime = time;
                }
            }
            */
        }

        public static void SaveRequested(Packet _packet)
        {
            //SRSingleton<GameContext>.Instance.AutoSaveDirector.SaveGame();
            /*
            MemoryStream memoryStream = new MemoryStream();
            {
                var _ASD = SRSingleton<GameContext>.Instance.AutoSaveDirector;
                _ASD.SavedGame.Save(memoryStream);
                memoryStream.Seek(0L, SeekOrigin.Begin);
            }

            SendData.SendSave(memoryStream);
            */
        }

        public static void HandleSave(Packet _packet)
        {
            var length = _packet.ReadInt();
            var array = _packet.ReadBytes(length);

            MemoryStream save = new MemoryStream(array);
            save.Seek(0L, SeekOrigin.Begin);

            //SavedGame_Load.SaveStream = save;
            //FileStorageProvider_GetGameData.HandleSave = true;
            //SRSingleton<GameContext>.Instance.AutoSaveDirector.BeginLoad(null, null, null);
        }

        public static void HandleLandPlotUpgrade(Packet _packet)
        {
            var id = _packet.ReadString();
            var upgrade = _packet.ReadInt();

            //if (SRSingleton<SceneContext>.Instance != null)
            //{
            //    if (SRSingleton<SceneContext>.Instance.GameModel.landPlots.TryGetValue(id, out LandPlotModel model))
            //    {
            //        if (model.gameObj != null)
            //        {
            //            var landPlot = model.gameObj.GetComponentInChildren<LandPlot>();
            //            landPlot.AddUpgrade((LandPlot.Upgrade)upgrade);
            //        }
            //    }
            //}
        }

        public static void HandleLandPlotReplace(Packet _packet)
        {
            var id = _packet.ReadString();
            var type = _packet.ReadInt();

            //if (SRSingleton<SceneContext>.Instance != null)
            //{
            //    if (SRSingleton<SceneContext>.Instance.GameModel.landPlots.TryGetValue(id, out LandPlotModel model))
            //    {
            //        if (model.gameObj != null)
            //        {
            //            var landPlotLocation = model.gameObj.GetComponentInChildren<LandPlotLocation>();
            //            var oldLandPlot = model.gameObj.GetComponentInChildren<LandPlot>();
            //            var replacementPrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPlotPrefab((LandPlot.Id)type);
            //            landPlotLocation.Replace(oldLandPlot, replacementPrefab);
            //        }
            //    }
            //}
        }

        public static void HandleSleep(Packet _packet)
        {
            var endTime = _packet.ReadDouble();

            //if (SRSingleton<LockOnDeath>.Instance != null)
            //{
            //    SRSingleton<LockOnDeath>.Instance.LockUntil(endTime, 0f);
            //}
        }

        public static void HandleCurrency(Packet _packet)
        {
            var value = _packet.ReadInt();

            //if (SRSingleton<SceneContext>.Instance != null)
            //{
            //    int difference = value - SRSingleton<SceneContext>.Instance.PlayerState._model.currency;
            //    SRSingleton<SceneContext>.Instance.PlayerState._model.currency = value;
            //    SRSingleton<PopupElementsUI>.Instance.CreateCoinsPopup(difference, PlayerState.CoinsType.NORM);
            //}
        }

        public static void HandleActors(Packet _packet)
        {
            int count = _packet.ReadInt();

            //Dictionary<long, IdentifiableModel> identifiables = null;
            //if (SRSingleton<SceneContext>.Instance != null)
            //{
            //    identifiables = SRSingleton<SceneContext>.Instance.GameModel.identifiables;
            //}

            for (int i = 0; i < count; i++)
            {
                var id = _packet.ReadInt();
                var position = _packet.ReadVector3();
                var rotation = _packet.ReadVector3();

                //if (identifiables != null)
                //{
                //    if (identifiables.ContainsKey(id))
                //    {
                //        if (HandleSlimes.Instance != null)
                //        {
                //            if (HandleSlimes.Instance.Positions.ContainsKey(id))
                //            {
                //                HandleSlimes.Instance.Positions[id] = position;
                //                HandleSlimes.Instance.Rotations[id] = rotation;
                //            }
                //            else
                //            {
                //                HandleSlimes.Instance.Positions.Add(id, position);
                //                HandleSlimes.Instance.Rotations.Add(id, rotation);
                //            }
                //        }
                //    }
                //}
            }
        }
    }
}
