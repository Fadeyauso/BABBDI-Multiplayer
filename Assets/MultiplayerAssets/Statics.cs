using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabbdiMultiplayer
{
    public static class Statics
    {
        public static bool IsMultiplayer = true;
        public static string SecondPlayerName = "None";
        public static bool Host;
        public static bool FriendInGame;
        public static bool JoinedTheGame;
        public static bool HandlePacket;
    }
}
