using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asker : MonoBehaviour
{
    [SerializeField] public DialogueObject secondDialogue;
    [SerializeField] private GameObject ticket;


    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 1) 
        {
            GetComponent<AmbientVoiceEmitter>().active = true;
            GetComponent<NPC>().dialogue[0] = secondDialogue;
        }
        else GetComponent<AmbientVoiceEmitter>().active = false;

        if (GetComponent<NPC>().dialogue[0] == secondDialogue && GetComponent<DialogueUI>().speaking && ticket != null && GameObject.Find("GameManager").GetComponent<GameManager>().haveTicket == 0 && !GameObject.Find("GameManager").GetComponent<GameManager>().endGame) ticket.SetActive(true);
    }
}
