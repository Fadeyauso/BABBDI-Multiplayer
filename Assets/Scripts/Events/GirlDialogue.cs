using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlDialogue : MonoBehaviour
{
    [SerializeField] public DialogueObject secondDialogue;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.impressGirl) 
        {
            GetComponent<NPC>().dialogue[0] = secondDialogue;
        }
    }
}
