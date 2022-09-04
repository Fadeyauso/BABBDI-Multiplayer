using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSFXVolume : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = GameObject.Find("SoundEffectSource").GetComponent<AudioSource>().volume;
    }
}
