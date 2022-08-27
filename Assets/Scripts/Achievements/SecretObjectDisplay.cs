using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretObjectDisplay : MonoBehaviour
{
    public int secretId;
    public string name;
    public bool collected;

    public Material masked;
    public Material displayed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collected) 
        {
            transform.Rotate(0,100 * Time.deltaTime,0);
            GetComponent<Renderer>().material = displayed;
        }
        else
        {
            GetComponent<Renderer>().material = masked;
        }
    }
}
