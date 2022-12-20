using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour
{
    private bool plugged;
    [SerializeField] private Button button;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetJoystickNames().Length == 0) plugged = false;

        if (Input.GetJoystickNames().Length != 0 && !plugged){
            if (Input.GetJoystickNames()[0] != "")
            {
                Debug.Log("allo");
                plugged = true;
                button.Select();
            }
        }
    }
}
