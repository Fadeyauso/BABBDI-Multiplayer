using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecChelouZone : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    
    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Player")
        {
            anim.SetBool("Bool1", true);
            SoundManager.Instance.PlaySound(open);
        }
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        
        anim.SetBool("Bool1", false);
        SoundManager.Instance.PlaySound(close);


    }
}
