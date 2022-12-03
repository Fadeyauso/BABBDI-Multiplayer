using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideCrosshair : MonoBehaviour
{
    private bool hide = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            if (Input.GetKeyDown("l")) {
                hide = !hide;
            }

        this.gameObject.GetComponent<Image>().enabled = hide;
    }
}
