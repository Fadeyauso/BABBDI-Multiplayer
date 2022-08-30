using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource, _indoorSource, _outdoorSource;
    [SerializeField] private float blendSpeed;

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

    public void PlayContinuousSound(AudioClip clop){
        _effectsSource.clip = clop;
        _effectsSource.Play();
    }

    public void PlayMusic(AudioClip clip, int loop){
        _musicSource.PlayOneShot(clip);
        if (loop == 1) _musicSource.loop = true;
        if (loop == 0) _musicSource.loop = false;
    }

    public void StopAllMusic()
    {

        _musicSource.Stop();
    }

    public void StopSound()
    {

        _effectsSource.Stop();
    }

    public void ChangeMasterVolume(float value){
        AudioListener.volume = value;
    }
    public void ChangeMusicVolume(float value){
        _musicSource.volume = value;
    }
    public void ChangeEffectVolume(float value){
        _effectsSource.volume = value;
    }

    public void IndoorBlend()
    {
        _indoorSource.volume = Mathf.Lerp(_indoorSource.volume, 1, blendSpeed * Time.deltaTime);
        _outdoorSource.volume = Mathf.Lerp(_outdoorSource.volume, 0, blendSpeed * Time.deltaTime);
    }
    public void OutdoorBlend()
    {
        _indoorSource.volume = Mathf.Lerp(_indoorSource.volume, 0, blendSpeed * Time.deltaTime);
        _outdoorSource.volume = Mathf.Lerp(_outdoorSource.volume, 1, blendSpeed * Time.deltaTime);
    }

    public void ToggleEffects(){
        _effectsSource.mute = !_effectsSource.mute;
    }

    public void ToggleMusic(){
        _musicSource.mute = !_musicSource.mute;
    }
}
