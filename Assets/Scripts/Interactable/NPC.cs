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
        if (!GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive) GameObject.Find("Player").GetComponent<FirstPersonController>().talkPopup.SetActive(true);
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
        GameObject.Find("Player").GetComponent<FirstPersonController>().talkPopup.SetActive(false);

    }
}
