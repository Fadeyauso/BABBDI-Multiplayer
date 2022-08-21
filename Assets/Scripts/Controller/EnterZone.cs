using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterZone : MonoBehaviour
{
    public AudioClip indoor;
    public AudioClip outdoor;
    // Start is called before the first frame update
    void Start()
    {
         SoundManager.Instance.PlayMusic(outdoor, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Door/Indoor")
        {
            SoundManager.Instance.StopAllMusic();
            SoundManager.Instance.PlayMusic(indoor, 1);
        }
        if (collisionInfo.tag == "Door/Outdoor")
        {
            SoundManager.Instance.StopAllMusic();
            SoundManager.Instance.PlayMusic(outdoor, 1);
        }
    }
}
