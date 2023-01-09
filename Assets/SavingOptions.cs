using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;

public class SavingOptions : MonoBehaviour, ISaveable
{
    public static SavingOptions Instance;
    public int speedrun;
    public int mouseInvertedX;
    public int mouseInvertedY;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public object SaveState()
    {
        return new SaveData()
        {
            speedrun = this.speedrun,
            mouseInvertedX = this.mouseInvertedX,
            mouseInvertedY = this.mouseInvertedY
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;

        speedrun = saveData.speedrun;
        mouseInvertedX = saveData.mouseInvertedX;
        mouseInvertedY = saveData.mouseInvertedY;
    
    }

    [Serializable]
    private struct SaveData
    {
        public int speedrun;
        public int mouseInvertedX;
        public int mouseInvertedY;

    }
}
