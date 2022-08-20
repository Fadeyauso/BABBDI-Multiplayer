using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource;

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip){
        _effectsSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip, int loop){
        _musicSource.volume = 1;
        _effectsSource.PlayOneShot(clip);
        if (loop == 1) _musicSource.loop = true;
        if (loop == 0) _musicSource.loop = false;
    }

    public void StopAllMusic()
    {
        while (_musicSource.volume > 0)
            _musicSource.volume -= 0.1f * Time.deltaTime;
    }

    public void ChangeMasterVolume(float value){
        AudioListener.volume = value;
    }

    public void ToggleEffects(){
        _effectsSource.mute = !_effectsSource.mute;
    }

    public void ToggleMusic(){
        _musicSource.mute = !_musicSource.mute;
    }
}
