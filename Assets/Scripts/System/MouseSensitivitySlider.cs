using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MouseSensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().lookSpeedX = _slider.value;
        GameObject.Find("Player").GetComponent<FirstPersonController>().lookSpeedY = _slider.value;
    }
}
