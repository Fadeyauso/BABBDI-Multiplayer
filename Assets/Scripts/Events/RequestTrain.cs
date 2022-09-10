using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTrain : Interactable
{
    private GameObject GameManager;
    private Material[] materials;
    public AudioClip train;
    public AudioClip sound;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

     public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (GameManager.GetComponent<GameManager>().haveTicket == 1)
        {
            GameManager.GetComponent<GameManager>().haveTicket = 0;
            GameManager.GetComponent<GameManager>().requestTrain = 1;
            SoundManager.Instance.PlaySound(sound);
            SoundManager.Instance.PlaySound(train);


            
        }
    }

    public override void OnLoseFocus()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
