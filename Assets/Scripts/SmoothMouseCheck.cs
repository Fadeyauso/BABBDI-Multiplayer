using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothMouseCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().smoothCam = GetComponent<Toggle>().isOn;
    }
}
