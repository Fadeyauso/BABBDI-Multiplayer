using GameServer;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabbdiMultiplayer
{
    public class UI : MonoBehaviour
    {
        #region Variables
        //Menu
        private bool menuState = true;
        private string menuMode;

        //Donators
        private WebClient webClient = new WebClient();
        private int donatorsCount;
        private List<string> donatorNames = new List<string>();
        private List<string> donatorAmounts = new List<string>();
        private int donatorsStartingPoint;
        #endregion

        void OnGUI()
        {
            //Unity documentation way
            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.BackQuote.ToString())))
            {
                menuState = !menuState;
            }

            GUI.color = Color.blue;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            if (menuState)
            {
                MainMenu();

                switch (menuMode)
                {
                    case "steam":
                        SteamLobby.Instance.SteamMenu();
                        break;
                    case "custom":
                        CustomLobby.Instance.CustomMenu();
                        break;
                    default:
                        WelcomeMenu();
                        break;
                }
            }
        }

        private void MainMenu()
        {
            #region BABBDIMP
            //GUI.Box(new Rect(10f, 10f, 160f, 300f), "<b>BABBDIMP</b>");
            #endregion

            #region Info
            //GUI.Box(new Rect(10f, 315f, 465f, 25f), "You can hide this menu by pressing the Tilde button (~, Ё, Ö)");
            #endregion
        }

        private void WelcomeMenu()
        {
            GUI.Label(new Rect(15f, 35f, 150f, 25f), "Choose your platform:");

            if (GUI.Button(new Rect(15f, 65f, 150f, 25f), "Steam"))
            {
                this.gameObject.AddComponent<SteamLobby>();
                menuMode = "steam";
            }

            if (GUI.Button(new Rect(15f, 95f, 150f, 25f), "Other (EGS, MS)"))
            {
                MultiplayerMain.Instance.SteamIsAvailable = false;
                this.gameObject.AddComponent<CustomLobby>();
                menuMode = "custom";
            }
        }

        void Start()
        {

            if (!MultiplayerMain.Instance.SteamIsAvailable)
            {
                this.gameObject.AddComponent<CustomLobby>();
                menuMode = "custom";
            }
        }
    }
}
