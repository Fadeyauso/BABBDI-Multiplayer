using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwaySound : MonoBehaviour
{
    private AudioSource audio;
    public GameObject[] lights;
    private GameManager gameManager;
    private AudioSource ambient;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ambient = GameObject.Find("AmbientSource").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //audio.mute = !GameObject.Find("GameManager").GetComponent<GameManager>().inSubway;

        if (gameManager.inSubway) audio.volume = Mathf.Lerp(audio.volume, ambient.volume, 1f * Time.deltaTime);
        else if (!gameManager.inSubway) audio.volume = Mathf.Lerp(audio.volume, 0, 2f * Time.deltaTime);

        if (gameManager.inSubway)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(true);
            }
        }
        else if (!gameManager.inSubway)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(false);
            }
        }
    }
}
