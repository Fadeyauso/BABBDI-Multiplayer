using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamerainverted : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Input.GetAxis("Mouse X") * 30, Input.GetAxis("Mouse Y") * 30, 0);

    }
}
