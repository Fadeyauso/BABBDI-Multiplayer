using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSFXVolume : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    private AudioSource proutit;
    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<AudioSource>() != null) audio = GetComponent<AudioSource>();
        proutit = GameObject.Find("SoundEffectSource").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = proutit.volume;
    }
}
