using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicVolume : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<AudioSource>() != null) audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = GameObject.Find("MusicSource").GetComponent<AudioSource>().volume;
    }
}
