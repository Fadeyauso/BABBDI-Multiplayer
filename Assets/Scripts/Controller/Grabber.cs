using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{

    public bool active;
    [SerializeField] public GameObject arrow;
    [SerializeField] public GameObject arrowInitPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.childCount > 1) active = false;
        else active = true;
        
    }


}
