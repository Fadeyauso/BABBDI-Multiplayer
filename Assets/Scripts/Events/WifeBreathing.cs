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
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.secondPart == 1) audio.mute = true;
    }
}
