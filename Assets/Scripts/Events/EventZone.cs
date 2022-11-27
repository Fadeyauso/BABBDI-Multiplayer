using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZone : MonoBehaviour
{

    [SerializeField] private bool oneTimeEvent = true;
    private bool done;

    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private Animator[] anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (oneTimeEvent)
            {
                if (!done) {
                    for (int i = 0; i < audioSource.Length; i++){
                        audioSource[i].Play();
                    }

                    for (int i = 0; i < anim.Length; i++){
                        anim[i].SetBool("Bool1", true);
                    }
                    
                    done = true;
                }
            }
        }
        
    }
}
