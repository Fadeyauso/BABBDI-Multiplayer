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
        GameObject.Find("Player").GetComponent<FirstPersonController>().noticketPopup.SetActive(true);
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
        else
        {
            GameManager.GetComponent<GameManager>().noticketPopup = true;
        }
    }

    public override void OnLoseFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().noticketPopup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
