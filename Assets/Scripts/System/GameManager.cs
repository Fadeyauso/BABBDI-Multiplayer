using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager Instance;

    [Header("UI")] 
    public GameObject pauseMenu;
    public GameObject confirmReturnHome;

    [SerializeField] public static int secretsFound = 0;
    [HideInInspector] public bool pickup;
    [HideInInspector] public int item;
    [HideInInspector] public bool inActivatedLift;
    [HideInInspector] public bool secretPopup;
    public int requestTrain = 0;
    [HideInInspector] public bool inSubway;
    [HideInInspector] public bool inClub;
    public bool endGame;
    public float gameTime = 0;
    public int secondPart;
    public int haveTicket;

    //Achievements
    [Header("Achievements")]
    public string lastAchievement;

    public bool wayClimber;
    public int wayClimberState;
    [SerializeField] private Toggle wayClimberToggle;


    [SerializeField] private AudioClip[] jingle = default;
    public bool popup;

    [Header("Saving & Loading")]
    public GameObject[] secrets;
    public int[] secretState;

    void Awake()
    {
        
        secretState = new int[secrets.Length];
        if (Instance != null) Instance = this;

        GetComponent<SaveLoadSystem>().Load();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReturnToMenu()
    {
        secondPart = 0;
        haveTicket = 0;
        requestTrain = 0;
        gameTime = 0;
        GetComponent<SaveLoadSystem>().Save();
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update() 
    {
        if (!endGame && !GameObject.Find("Player").GetComponent<FirstPersonController>().pause) gameTime += Time.deltaTime;

        wayClimberToggle.isOn = wayClimber;

        lobbyTimer -= Time.deltaTime;
        if (lobbyTimer > 0)
        {
            pauseMenu.SetActive(false);
            confirmReturnHome.SetActive(false);
        }
    }

    public void AddSecret()
    {
        secretsFound ++;
    }
    private float lobbyTimer;

   

    public void Popup() 
    {
        popup = true;
        SoundManager.Instance.PlaySound(jingle[Random.Range(0, jingle.Length - 1)]);
    }

    public void ReturnHome()
    {
        GameObject.Find("Player").transform.position = GameObject.Find("LobbyPosition").transform.position;
        lobbyTimer = 0.1f;
    }

    //Saving Process
    
    public object SaveState()
    {
        return new SaveData()
        {
            ticket = this.haveTicket,
            parttwo = this.secondPart,
            endtrain = this.requestTrain,
            playTime = this.gameTime,

            //Achievements
            wayClimber = this.wayClimberState,

            //Secrets
            secret01 = this.secretState[0]
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;

        haveTicket = saveData.ticket;
        secondPart = saveData.parttwo;
        requestTrain = saveData.endtrain;
        gameTime = saveData.playTime;

        //Achievements
        wayClimberState = saveData.wayClimber;
        if (wayClimberState == 1) wayClimber = true;
        
        //Secrets
        this.secretState[0] = saveData.secret01;
    }

    [Serializable]
    private struct SaveData
    {
        public int ticket;
        public int parttwo;
        public int endtrain;
        public float playTime;

        //Achievements
        public int wayClimber;

        //Secrets
        public int secret01;

    }
}
