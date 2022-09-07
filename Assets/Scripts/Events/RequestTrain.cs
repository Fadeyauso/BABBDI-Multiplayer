using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTrain : Interactable
{
    private GameObject GameManager;
    private Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

     public override void OnFocus()
    {
        foreach (Material mat in materials)
        {
            mat.SetFloat("_OutlineWidth", 0.015f);
        }
    }

    public override void OnInteract()
    {
        if (GameManager.GetComponent<GameManager>().haveTicket == 1)
        {
            GameManager.GetComponent<GameManager>().haveTicket = 0;
            GameManager.GetComponent<GameManager>().requestTrain = 1;


            
        }
    }

    public override void OnLoseFocus()
    {
        foreach (Material mat in materials)
        {
            mat.SetFloat("_OutlineWidth", 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
