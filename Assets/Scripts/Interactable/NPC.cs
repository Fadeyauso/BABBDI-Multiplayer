using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public AudioClip interactionClip;
    public int npcId;

    public DialogueObject[] dialogue;

    public override void OnFocus()
    {
        Debug.Log("asccaca");
    }

    public override void OnInteract()
    {
        SoundManager.Instance.PlaySound(interactionClip);
        if (GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive == false) GetComponent<DialogueUI>().ShowDialogue(dialogue[0]);
        
    }

    public override void OnLoseFocus()
    {
        Debug.Log("aaaa");

    }
}
