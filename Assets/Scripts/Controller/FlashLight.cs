using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<InteractObject>().inHands)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1"))
            {
                light.SetActive(!light.activeSelf);
            }
        }
    }
}
