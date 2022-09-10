using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioClip music;
    public float countown;
    public float timer;

    void Update()
    {
        timer -= Time.deltaTime;
    }
}
