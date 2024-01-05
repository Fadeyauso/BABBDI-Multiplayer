using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private AudioClip pointer;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioSource audio;

    public void PlayGame()
    {
        SceneManager.LoadScene("Scene_A");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnPointerEnter()
    {
        audio.PlayOneShot(pointer);
        
    }

    public void OnClick()
    {
        audio.PlayOneShot(click);
    }

    private void Start()
    {
        GameObject.Find("SavingOptions").GetComponent<SaveLoadSystem>().Load();
    }

    public void MultiplayerClicked()
    {

    }

    

}
