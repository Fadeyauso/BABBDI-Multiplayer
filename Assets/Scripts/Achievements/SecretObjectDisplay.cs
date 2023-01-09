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

    public Vector3 rotation;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.secretState[secretId] == 1) collected = true;
        else collected = false;

        if (collected) 
        {
            transform.Rotate(rotation.x * Time.deltaTime, rotation.y * Time.deltaTime,rotation.z * Time.deltaTime);
            GetComponent<Renderer>().material = displayed;
        }
        else
        {
            GetComponent<Renderer>().material = masked;
        }
    }
}
