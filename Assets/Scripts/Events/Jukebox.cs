using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : Interactable
{
    public override void OnFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().jukeboxPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        GetComponent<AudioSource>().mute = !GetComponent<AudioSource>().mute;
    }

    public override void OnLoseFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().jukeboxPopup.SetActive(false);
    }
}
