using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Object;

namespace BabbdiMultiplayer
{
    public class Loader : MonoBehaviour
    {
        public const string ModVersion = "0.1.0";

        private void Awake()
        {
            CreateBABBDIMP();
        }

        private void CreateBABBDIMP()
        {
            var BABBDIMP = gameObject;
            BABBDIMP.AddComponent<MultiplayerMain>();
            BABBDIMP.AddComponent<UI>();
            DontDestroyOnLoad(BABBDIMP);
            //HarmonyInstance.PatchAll();
        }
    }
}
