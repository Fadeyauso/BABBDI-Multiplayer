using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeBreathing : MonoBehaviour
{
    private AudioSource audio;
    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 1) audio.mute = true;
    }
}
