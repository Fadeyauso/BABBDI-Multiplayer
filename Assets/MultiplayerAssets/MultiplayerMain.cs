using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BabbdiMultiplayer
{
    public class MultiplayerMain : MonoBehaviour
    {
        #region Variables
        public static MultiplayerMain Instance;

        //Main objects
        public FirstPersonController Player;
        private GameObject _player;

        //Game loading
        public bool GameLoadCheck = true;
        public bool GameIsLoaded;

        //Stuff
        public bool SteamIsAvailable;
        #endregion

        void Start()
        {
            Instance = this;
            SteamIsAvailable = SteamAPI.Init();
        }

        void Update()
        {
            //Game loading
            if (GameLoadCheck)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                if (activeScene.name.Equals("Scene_AAA"))
                {
                    GetPlayerModel();
                    GameIsLoaded = true;
                    GameLoadCheck = false;
                }
            }
        }

        void FixedUpdate()
        {
            if (Player == null)
            {
                Player = FindObjectOfType<FirstPersonController>();
                if (Player != null)
                {
                    HandlePlayer();
                }
            }
        }

        private void GetPlayerModel()
        {
            _player = GameObject.Find("SM_BigDoll");//GameObject.Find("Player");
            _player = Instantiate(_player);
            var itemContainer = new GameObject("itemContainer");
            itemContainer.transform.parent = _player.transform;
            itemContainer.transform.localPosition = Vector3.zero;

            _player.AddComponent<Movement>();
            _player.AddComponent<BABBDIPlayer>();

            DontDestroyOnLoad(_player);
        }

        private void HandlePlayer()
        {
            Player.gameObject.AddComponent<ReadData>();
        }
    }
}
