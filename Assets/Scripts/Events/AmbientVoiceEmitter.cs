using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientVoiceEmitter : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float playFrequency;
    private float timer;
    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = playFrequency;
            
            audio.PlayOneShot(sound);
        }

        if (GetComponent<DialogueUI>() != null)
        {
            if (GetComponent<DialogueUI>().speaking) audio.mute = true;
            else audio.mute = false;
        }


    }
}
