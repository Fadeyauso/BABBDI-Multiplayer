using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : Interactable
{
    [SerializeField] private Material on;
    [SerializeField] private Material off;
    [SerializeField] private GameObject tv;
    [SerializeField] private AudioClip tvClip;
    private Material[] mats;
    private bool turnedOn = true;

    private void Awake()
    {

    }

    public override void OnFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().jukeboxPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        SoundManager.Instance.PlaySound(tvClip);
        mats = tv.GetComponent<Renderer>().materials;
        
        turnedOn = !turnedOn;

        if (turnedOn) mats[3] = on;
        else if (!turnedOn) mats[3] = off;

        tv.GetComponent<Renderer>().materials = mats;
    }

    public override void OnLoseFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().jukeboxPopup.SetActive(false);
    }

}
