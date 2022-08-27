using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private AudioClip pointer;
    [SerializeField] private AudioSource audio;

    public void PlayGame()
    {
        SceneManager.LoadScene("Scene_ACOPY");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnPointerEnter()
    {
        audio.PlayOneShot(pointer);
    }

}
