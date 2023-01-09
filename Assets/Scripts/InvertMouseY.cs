using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertMouseY : MonoBehaviour
{
    private FirstPersonController player;
    private SavingOptions save;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FirstPersonController>();
        save = GameObject.Find("SavingOptions").GetComponent<SavingOptions>();
        if (save.mouseInvertedY == 1) {
            GetComponent<Toggle>().isOn = true;
            player.invertY = true;
        }
        else{
            GetComponent<Toggle>().isOn = false;
            player.invertY = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        player.invertY = GetComponent<Toggle>().isOn;
        if (player.invertY) save.mouseInvertedY = 1;
        else save.mouseInvertedY = 0;
    }
}
