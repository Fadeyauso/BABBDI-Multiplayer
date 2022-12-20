using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : Interactable
{
    [SerializeField] private AudioClip clip;

    public override void OnFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().jukeboxPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        SoundManager.Instance.PlaySound(clip);
        GetComponent<AudioSource>().mute = !GetComponent<AudioSource>().mute;
    }

    public override void OnLoseFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().jukeboxPopup.SetActive(false);
    }
}
