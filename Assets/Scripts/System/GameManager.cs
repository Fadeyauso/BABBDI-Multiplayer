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
    [HideInInspector] public bool inActivatedLift;

    //Achievements
    [Header("Achievements")]
    public bool wayClimber;
    public string lastAchievement;
    [SerializeField] private Toggle wayClimberToggle;


    [SerializeField] private AudioClip[] jingle = default;
    public bool popup;

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

    public void Popup() 
    {
        popup = true;
        SoundManager.Instance.PlaySound(jingle[Random.Range(0, jingle.Length - 1)]);
    }
}
