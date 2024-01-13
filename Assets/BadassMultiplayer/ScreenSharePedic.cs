using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSharePedic : MonoBehaviour
{
    public Image theImage;

    private void Start()
    {
        theImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            theImage.gameObject.SetActive(true);
        }
        else
        {
            theImage.gameObject.SetActive(false);
        }
    }
}
