using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedrunMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Parameters") != null)
        {
            GetComponent<Toggle>().isOn = GameObject.Find("Parameters").GetComponent<Parameters>().speedrun;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
