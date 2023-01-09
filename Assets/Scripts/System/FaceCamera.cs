using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private FirstPersonController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FirstPersonController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAtPosition = GameObject.Find("Player").transform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);

    }
}
