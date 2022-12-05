using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerControlsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetJoystickNames()[0] != "") panel.SetActive(true);
        else
            panel.SetActive(false);
    }
}
