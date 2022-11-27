using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public int npcId;
    public bool hasSecondPhase;
    public bool interactedWith;
    public bool dialogueDone; 
    [SerializeField] public GameObject exclamation;

    public DialogueObject[] dialogue;
    public DialogueObject[] dialogue2;
    public int dialogueIndex = 0;
    public int dialogueIndex2 = 0;
    public int secondPhaseDialogues = 0;
    public int dialogue2Number = 2;

    public override void OnFocus()
    {
        if (!GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive) GameObject.Find("Player").GetComponent<FirstPersonController>().talkPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        dialogueIndex = Mathf.Clamp(dialogueIndex, 0, dialogue.Length-1);
        dialogueIndex2 = Mathf.Clamp(dialogueIndex2, 0, dialogue2.Length-1);

        if (GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 1 && hasSecondPhase && GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive == false)
        {
            dialogueDone = false;
            GetComponent<DialogueUI>().ShowDialogue(dialogue2[dialogueIndex2]);
            if (dialogueIndex2 >= dialogue2.Length-1) dialogueIndex2 = 0;
            else dialogueIndex2 ++;

            secondPhaseDialogues ++;
        }
        else if (GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive == false) 
        {
            GetComponent<DialogueUI>().ShowDialogue(dialogue[dialogueIndex]);
            if (dialogueIndex >= dialogue.Length-1) {
                dialogueIndex = 0;
                dialogueDone = true; 
            }
            else dialogueIndex ++;
        }

        if (!interactedWith)
        {
            interactedWith = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().npcInteractedWith ++;
        }

        
    }

    public override void OnLoseFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().talkPopup.SetActive(false);

    }

    void Start()
    {
        dialogue2Number = dialogue2.Length;
    }

    void Update()
    {
        if (exclamation != null)
        {
            if (!GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive && (dialogue.Length-1 == 0 ? !interactedWith : dialogueIndex > 0) && (hasSecondPhase ? GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 0 : 1==1) && !dialogueDone) exclamation.SetActive(true);
            else if (!GameObject.Find("Player").GetComponent<FirstPersonController>().dialogueActive && (GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 1) && hasSecondPhase && secondPhaseDialogues < dialogue2Number) exclamation.SetActive(true);
            else exclamation.SetActive(false);
        }
        
    }
}
