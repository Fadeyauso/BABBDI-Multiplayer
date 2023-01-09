using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTrain : Interactable
{
    private Material[] materials;
    public AudioClip train;
    public AudioClip sound;
    // Start is called before the first frame update

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public override void OnFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().noticketPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        if (gameManager.haveTicket == 1)
        {
            gameManager.haveTicket = 0;
            gameManager.requestTrain = 1;
            SoundManager.Instance.PlaySound(sound);
            SoundManager.Instance.PlaySound(train);
            GetComponent<AudioSource>().Play();


            
        }
        else
        {
            gameManager.noticketPopup = true;
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
