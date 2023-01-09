using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertMouseX : MonoBehaviour
{
    private FirstPersonController player;
    private SavingOptions save;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FirstPersonController>();
        save = GameObject.Find("SavingOptions").GetComponent<SavingOptions>();
        if (save.mouseInvertedX == 1) {
            GetComponent<Toggle>().isOn = true;
            player.invertX = true;
        }
        else {
            GetComponent<Toggle>().isOn = false;
            player.invertX = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        player.invertX = GetComponent<Toggle>().isOn;
        if (player.invertX) save.mouseInvertedX = 1;
        else save.mouseInvertedX = 0;
    }
}
