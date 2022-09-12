using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource, _ambientSource, _indoorSource, _outdoorSource;
    [SerializeField] private float blendSpeed;
    private bool outdoor = true;
    private bool indoor = false;

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
    }

    public void PlaySound(AudioClip clip){
        _effectsSource.PlayOneShot(clip);
    }

    public void PlayAmbientSound(AudioClip clip){
        _ambientSource.PlayOneShot(clip);
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
    public void ChangeAmbientVolume(float value){
        _ambientSource.volume = value;

        if (indoor) _indoorSource.volume = _ambientSource.volume;
        if (outdoor) _outdoorSource.volume = _ambientSource.volume;
    }

    public void IndoorBlend()
    {
        indoor = true;
        outdoor = false;
        _indoorSource.volume = Mathf.Lerp(_indoorSource.volume, _ambientSource.volume, blendSpeed * Time.deltaTime);
        _outdoorSource.volume = Mathf.Lerp(_outdoorSource.volume, 0, blendSpeed * Time.deltaTime);
    }
    public void OutdoorBlend()
    {
        indoor = false;
        outdoor = true;
        _indoorSource.volume = Mathf.Lerp(_indoorSource.volume, 0, blendSpeed * Time.deltaTime);
        _outdoorSource.volume = Mathf.Lerp(_outdoorSource.volume, _ambientSource.volume, blendSpeed * Time.deltaTime);
    }

    public void ToggleEffects(){
        _effectsSource.mute = !_effectsSource.mute;
    }

    public void ToggleMusic(){
        _musicSource.mute = !_musicSource.mute;
    }
}
