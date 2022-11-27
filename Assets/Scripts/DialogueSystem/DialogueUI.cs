using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public string name;
    public int NPCid;
    public bool speaking;

    public AudioClip[] voiceClip;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public TMP_Text textLabel;
    [SerializeField] public TMP_Text name_label;
    [SerializeField] public DialogueObject currentDialogue;
    public GameObject continueDialogueFX;

    private TypewriterEffect typewriterEffect;

    void Awake()
    {
        continueDialogueFX = GameObject.Find("continueDialogueFX");
    }

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        speaking = true;
        dialogueBox.SetActive(true);
        continueDialogueFX.SetActive(false);
        if (GetComponent<Dragoyevic>() != null) GetComponent<Dragoyevic>().giveCompass = true;
        name_label.text = name;
        SoundManager.Instance.PlaySound(voiceClip[Random.Range(0, voiceClip.Length - 1)]);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;
            

            yield return null;
            continueDialogueFX.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1"));
            continueDialogueFX.SetActive(false);
            
        }
        if (GetComponent<Dragoyevic>() != null && activ == true && GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 1) 
        {
            activ = false;
            GetComponent<Dragoyevic>().activateBridge = true;
        }

        CloseDialogueBox();
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning){
            yield return null;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1"))
            {
                continueDialogueFX.SetActive(true);
                typewriterEffect.Stop();
            }
        }
    }

    private bool activ = true;

    private void CloseDialogueBox()
    {
        speaking = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        //player.GetComponent<CollectItems>().dialogueActive = false;
        if ((!GetComponent<NPC>().dialogueDone) && (GetComponent<NPC>().dialogue.Length-1 == 0 ? !GetComponent<NPC>().interactedWith : GetComponent<NPC>().dialogueIndex > 0) && (GetComponent<NPC>().hasSecondPhase ? GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 0 : 1==1) && GameObject.Find("GameManager").GetComponent<GameManager>().startTimer < -1) 
                SoundManager.Instance.PlaySound(GetComponent<NPC>().exclamation.GetComponent<ExclamationPoint>().initClip);

        
    }
}
