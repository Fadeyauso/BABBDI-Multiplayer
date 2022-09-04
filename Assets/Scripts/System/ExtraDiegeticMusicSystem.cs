using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraDiegeticMusicSystem : MonoBehaviour
{
    [SerializeField] private AudioClip[] music = default;
    private float timer = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            PlayRandomMusic();
        }
    }   

    private void PlayRandomMusic()
    {
        timer = Random.Range(150, 300);
        //SoundManager.Instance.PlayMusic(music[Random.Range(0, music.Length - 1)], 0);
    }
}
