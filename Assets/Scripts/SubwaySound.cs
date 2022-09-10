using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwaySound : MonoBehaviour
{
    private AudioSource audio;
    public GameObject[] lights;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audio.mute = !GameObject.Find("GameManager").GetComponent<GameManager>().inSubway;

        if (GameObject.Find("GameManager").GetComponent<GameManager>().inSubway)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(true);
            }
        }
        else if (!GameObject.Find("GameManager").GetComponent<GameManager>().inSubway)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(false);
            }
        }
    }
}
