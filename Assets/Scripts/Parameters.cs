using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parameters : MonoBehaviour
{
    public static Parameters Instance;
    public bool speedrun;
    public SavingOptions save;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        save = GameObject.Find("SavingOptions").GetComponent<SavingOptions>();
        
    }

    // Update is called once per frame
    void Start()
    {
        if (save.speedrun == 1) speedrun = true;
        else speedrun = false;
    }

    public void ValueChange(Toggle toggle)
    {
        speedrun = toggle.isOn;
        if (speedrun) save.speedrun = 1;
        else save.speedrun = 0;
    }
}
