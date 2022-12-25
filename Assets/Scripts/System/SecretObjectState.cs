using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretObjectState : MonoBehaviour
{
    [SerializeField] private bool collected;
    [SerializeField] private int objectIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().secretState[objectIndex] == 1) collected = true;
        else collected = false;

        if (collected) 
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
