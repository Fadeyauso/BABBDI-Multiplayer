using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubSound : MonoBehaviour
{
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().inClub) audio.volume = Mathf.Lerp(audio.volume, GameObject.Find("AmbientSource").GetComponent<AudioSource>().volume, 1f * Time.deltaTime);
        else if (!GameObject.Find("GameManager").GetComponent<GameManager>().inClub) audio.volume = Mathf.Lerp(audio.volume, 0, 3f * Time.deltaTime);
    }
}
