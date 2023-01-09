using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asker : MonoBehaviour
{
    [SerializeField] public DialogueObject secondDialogue;
    [SerializeField] private GameObject ticket;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.secondPart == 1 && gameManager.haveTicket == 0) 
        {
            GetComponent<AmbientVoiceEmitter>().active = true;
            GetComponent<NPC>().dialogue[0] = secondDialogue;
        }
        else GetComponent<AmbientVoiceEmitter>().active = false;

        if (GetComponent<NPC>().dialogue[0] == secondDialogue && GetComponent<DialogueUI>().speaking && ticket != null && gameManager.haveTicket == 0 && !gameManager.endGame) ticket.SetActive(true);
    }
}
