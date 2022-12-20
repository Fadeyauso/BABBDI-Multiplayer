using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVSound : MonoBehaviour
{
    [SerializeField] private GameObject tv;
    private AudioSource audio;

    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audio.mute = !tv.GetComponent<TV>().turnedOn;
    }
}
