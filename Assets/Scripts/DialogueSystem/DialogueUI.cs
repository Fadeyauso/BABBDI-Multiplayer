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
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text name_label;
    [SerializeField] public DialogueObject currentDialogue;

    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        speaking = true;
        dialogueBox.SetActive(true);
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
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F));
        }

        CloseDialogueBox();
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning){
            yield return null;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
            {
                typewriterEffect.Stop();
            }
        }
    }

    private void CloseDialogueBox()
    {
        speaking = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        //player.GetComponent<CollectItems>().dialogueActive = false;
    }
}
