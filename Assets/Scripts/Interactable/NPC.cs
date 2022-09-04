using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public AudioClip interactionClip;
    public int npcId;

    public DialogueObject[] dialogue;
    public int dialogueIndex = -1;

    public override void OnFocus()
    {
        Debug.Log("asccaca");
    }

    public override void OnInteract()
    {
        dialogueIndex = Mathf.Clamp(dialogueIndex, 0, dialogue.Length-1);
        SoundManager.Instance.PlaySound(interactionClip);
        if (GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive == false) 
        {
            GetComponent<DialogueUI>().ShowDialogue(dialogue[dialogueIndex]);
            if (dialogueIndex >= dialogue.Length-1) dialogueIndex = 0;
            else dialogueIndex ++;
        }
        
    }

    public override void OnLoseFocus()
    {
        Debug.Log("aaaa");

    }
}
