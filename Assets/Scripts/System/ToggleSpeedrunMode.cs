using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSpeedrunMode : MonoBehaviour
{
    public GameObject speedrunLabel;
    private SavingOptions save;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Parameters") != null)
        {
            GetComponent<Toggle>().isOn = GameObject.Find("Parameters").GetComponent<Parameters>().speedrun;
        }
        save = GameObject.Find("SavingOptions").GetComponent<SavingOptions>();
    }

    // Update is called once per frame
    void Update()
    {
        speedrunLabel.SetActive(GetComponent<Toggle>().isOn); 

        if (GetComponent<Toggle>().isOn) save.speedrun = 1;
        else save.speedrun = 0;

        GameObject.Find("Parameters").GetComponent<Parameters>().speedrun = GetComponent<Toggle>().isOn;

    }

}
