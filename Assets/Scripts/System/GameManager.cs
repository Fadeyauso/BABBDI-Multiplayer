using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public static int secretsFound = 0;
    [HideInInspector] public bool pickup;
    [HideInInspector] public int item;

    //Achievements
    [Header("Achievements")]
    public bool wayClimber;
    [SerializeField] private Toggle wayClimberToggle;

    void Awake()
    {
        if (Instance != null) Instance = this;
        DontDestroyOnLoad(this);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() 
    {
        wayClimberToggle.isOn = wayClimber;
    }

    public void AddSecret()
    {
        secretsFound ++;
    }
}
