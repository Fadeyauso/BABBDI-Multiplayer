using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlDialogue : MonoBehaviour
{
    [SerializeField] public DialogueObject secondDialogue;


    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().impressGirl) 
        {
            GetComponent<NPC>().dialogue[0] = secondDialogue;
        }
    }
}
