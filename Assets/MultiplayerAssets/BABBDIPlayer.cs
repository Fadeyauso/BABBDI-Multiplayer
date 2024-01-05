using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabbdiMultiplayer
{
    public class BABBDIPlayer : MonoBehaviour
    {
        public static BABBDIPlayer Instance;

        public Movement BABBDIPlayerMovement;

        void Start()
        {
            Instance = this;

            BABBDIPlayerMovement = GetComponent<Movement>();
        }
    }
}
