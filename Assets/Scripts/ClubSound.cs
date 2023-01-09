using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubSound : MonoBehaviour
{
    private AudioSource audio;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.inClub) audio.volume = Mathf.Lerp(audio.volume, GameObject.Find("AmbientSource").GetComponent<AudioSource>().volume, 1f * Time.deltaTime);
        else if (!gameManager.inClub) audio.volume = Mathf.Lerp(audio.volume, 0, 3f * Time.deltaTime);
    }
}
