using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public GameObject firstPoint;
    public GameObject secondPoint;

    public bool isTriggered;
    public bool stop;
    public float speed = 0.01f;

    public bool topReach = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered && transform.position.y < secondPoint.transform.position.y && topReach == false) 
        {  
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            stop = true;
        }
        else if (isTriggered && transform.position.y > firstPoint.transform.position.y && topReach == true) 
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            stop = true;
        }

        if (isTriggered && transform.position.y > secondPoint.transform.position.y && stop == true || isTriggered && transform.position.y < firstPoint.transform.position.y && stop == true) 
        {
            topReach = !topReach;
            isTriggered = false;
            stop = false;
        }
    }
}
