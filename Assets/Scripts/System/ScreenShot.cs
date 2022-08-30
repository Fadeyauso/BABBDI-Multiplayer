using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Random=UnityEngine.Random;

public class ScreenShot : MonoBehaviour
{

    private string directoryName = "Screenshots";
    private string screenshotName = "";

    [SerializeField] private AudioClip screenshotClip;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12)) TakeScreenshot();
    }

    string RandomStringGenerator(int length)
    {
        string characters = "ABDIBB";
        string generated_string = "";

        for(int i = 0; i < length; i++)
            generated_string += characters[Random.Range(0, length)];

        return generated_string;
    }

    public void TakeScreenshot()
    {
        string timeNow = DateTime.Now.ToString("dd-MMMM-yyyy HHmmss");

        SoundManager.Instance.PlaySound(screenshotClip);

        screenshotName = RandomStringGenerator(6);
        
        
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        string screenshotPath = Path.Combine(documentsPath, Application.productName, directoryName);

        DirectoryInfo screenshotDirectory = Directory.CreateDirectory(screenshotPath);

        ScreenCapture.CaptureScreenshot(Path.Combine(screenshotDirectory.FullName, screenshotName + "(" + timeNow + ")" + ".png"));
        return;
    }       

    
}
