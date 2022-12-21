using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager Instance;
    public bool gamepad;
    public AudioClip destroy;

    [Header("UI")] 
    public GameObject pauseMenu;
    public GameObject confirmReturnHome;
    public GameObject notavailable;
    public GameObject returnoffice;
    public GameObject speedrunLabel;
    public GameObject ticketDisplay;
    public GameObject mapObjectDisplay;
    public GameObject mapButtonDisplay;
    public GameObject qualitySettings;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text name_label;

    [Header("Cutscene")] 
    public Animator anim1;
    public Animator anim2;
    public GameObject bridgeCam;
    public GameObject userInterface;
    public GameObject userInterface1;
    public AudioSource bridgeAudio;

    [Header("Save Object in hands")]
    public int club;
    public int climber;
    public int propeller;
    public int blower;
    public int flashlight;
    public int soap;
    public int ball;
    public int bigball;
    public int stick;
    public int grabber;
    public int motorBike;
    public int compass;
    public int trumpet;
    public int secretfinder;

    [Header("MAINLINE")]
    public GameObject dragoStart;
    public GameObject dragoEnd;
    private bool instantiate = true;
    public Transform secondPosition;
    public GameObject continueDialogueFX;


    public int secretsFound = 0;
    public int npcInteractedWith;
    public int maxNpc;
    [HideInInspector] public bool pickup;
    [HideInInspector] public int item;
    [HideInInspector] public bool inActivatedLift;
    [HideInInspector] public bool secretPopup;
    [HideInInspector] public bool mapPopup;
    [HideInInspector] public bool noticketPopup;
    [HideInInspector] public bool savePopup;
    public int requestTrain = 0;
    [HideInInspector] public bool inSubway;
    [HideInInspector] public bool inClub;
    public bool endGame;
    public float gameTime = 0;
    public int secondPart;
    public int haveTicket;
    public int haveMap;
    public float startTimer;

    //Achievements
    [Header("Achievements")]
    public string lastAchievement;

    public bool wayClimber;
    public int wayClimberState;
    [SerializeField] private Toggle wayClimberToggle;
    public bool trainDeath;
    public int trainDeathState;
    [SerializeField] private Toggle trainDeathToggle;
    public bool allSecrets;
    public int allSecretsState;
    [SerializeField] private Toggle allSecretsToggle;
    public bool interactEveryone;
    public int interactEveryoneState;
    [SerializeField] private Toggle interactEveryoneToggle;
    public bool playDog;
    public int playDogState;
    [SerializeField] private Toggle playDogToggle;
    public bool gameUnder;
    public int gameUnderState;
    [SerializeField] private Toggle gameUnderToggle;
    public bool impressGirl;
    public int impressGirlState;
    [SerializeField] private Toggle impressGirlToggle;
    public bool bikeAir;
    public int bikeAirState;
    [SerializeField] private Toggle bikeAirToggle;
    public bool escapeBabbdi;
    public int escapeBabbdiState;
    [SerializeField] private Toggle escapeBabbdiToggle;
    public bool ticket;
    public int ticketState;
    [SerializeField] private Toggle ticketToggle;


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
        if (GameObject.Find("Parameters") != null)
        {
            speedrunLabel.SetActive(GameObject.Find("Parameters").GetComponent<Parameters>().speedrun); 
        }
        qualitySettings.GetComponent<SetQuality>().SetQualityLevelDropdown(3);

        instantiate = true;

        if (haveMap == 1) mapObjectDisplay.SetActive(false);
    }

    public void ReturnToMenu()
    {
        haveTicket = 0;
        secondPart = 0;
        requestTrain = 0;
        gameTime = 0;
        GameObject.Find("Player").transform.position = new Vector3(158.621f, 42.73f, -31.9f);
        GameObject.Find("Player").transform.rotation = Quaternion.Euler(0,0,0);
        GameObject.Find("Player").GetComponent<FirstPersonController>().rotationX = 0;
        GetComponent<SaveLoadSystem>().Save();
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame#
    public float bridgeTimer;
    void Update() 
    {
        if (secretsFound >= 21) 
        {
            if (!allSecrets) Popup();
            allSecrets = true;
            allSecretsState = 1;
            lastAchievement = "Secrets master";
            SteamIntegration.Instance.UnlockAchivement("SecretsMaster");
        }

        if (Input.GetJoystickNames().Length != 0)
            if (Input.GetJoystickNames()[0] != "") gamepad = true;
        else gamepad = false;
        bridgeTimer -= Time.deltaTime;

        if (haveTicket == 1) ticket = true;
        else ticket = false;
        startTimer -= Time.deltaTime;
        if (!endGame && !GameObject.Find("Player").GetComponent<FirstPersonController>().pause) gameTime += Time.deltaTime;

        wayClimberToggle.isOn = wayClimber;
        allSecretsToggle.isOn = allSecrets;
        interactEveryoneToggle.isOn = interactEveryone;
        gameUnderToggle.isOn = gameUnder;
        escapeBabbdiToggle.isOn = escapeBabbdi;
        bikeAirToggle.isOn = bikeAir;
        playDogToggle.isOn = playDog;
        impressGirlToggle.isOn = impressGirl;
        trainDeathToggle.isOn = trainDeath;
        ticketToggle.isOn = ticket;


        lobbyTimer -= Time.deltaTime;
        if (lobbyTimer > 0)
        {
            pauseMenu.SetActive(false);
            confirmReturnHome.SetActive(false);
        }

        if (secondPart == 1) {
            notavailable.SetActive(true);
            returnoffice.SetActive(true);
        }
        else {
            notavailable.SetActive(false);
            returnoffice.SetActive(false);
        }

        if (!GameObject.Find("Player").GetComponent<FirstPersonController>().pause && haveTicket == 1)
        {
            ticketDisplay.SetActive(true);
        }
        else ticketDisplay.SetActive(false);

        if (npcInteractedWith == maxNpc)
        {
            if (!interactEveryone) Popup();
            interactEveryone = true;
            interactEveryoneState = 1;
            lastAchievement = "Social Quest";
            SteamIntegration.Instance.UnlockAchivement("SocialQuest");
        }

        if (endGame)
        {
            if (gameTime < 240 && !gameUnder)
            {
                Popup();
                gameUnder = true;
                gameUnderState = 1;
                lastAchievement = "Way of the rusher";
                SteamIntegration.Instance.UnlockAchivement("WayOfTheRusher");
            }
            if (!escapeBabbdi){
                Popup();
                escapeBabbdi = true;
                escapeBabbdiState = 1;
                lastAchievement = "Melancholic departure";
                SteamIntegration.Instance.UnlockAchivement("MelancholicDeparture");
            }
        }
        if (secondPart == 1 && instantiate == true) Invoke("drago", 1);

        
        if (haveMap == 0) mapButtonDisplay.SetActive(false);
        else mapButtonDisplay.SetActive(true);
        
    }
    private void drago()
    {

        if (secondPart == 1 && instantiate == true)
        {
            instantiate = false;
            Destroy(dragoStart);
            var obj = Instantiate(dragoEnd, secondPosition.position, secondPosition.rotation);
            obj.GetComponent<DialogueUI>().dialogueBox = dialogueBox;
            obj.GetComponent<DialogueUI>().textLabel = textLabel;
            obj.GetComponent<DialogueUI>().name_label = name_label;
            obj.GetComponent<DialogueUI>().continueDialogueFX = continueDialogueFX;


        }
    }

    public void AddSecret()
    {
        secretsFound ++;
    }
    private float lobbyTimer;

   
    [ContextMenu("Popup")]
    public void Popup() 
    {
        if (!endGame)
        {
            popup = true;
            SoundManager.Instance.PlaySound(jingle[Random.Range(0, jingle.Length - 1)]);
        }
        else{
            Debug.Log("qlqlq");
        }
    }

    public void ReturnHome()
    {
        if (secondPart == 0)
        {
            GameObject.Find("Player").transform.position = GameObject.Find("LobbyPosition").transform.position;
            lobbyTimer = 0.1f;
        }
        
    }
    public void ReturnOffice()
    {
        GameObject.Find("Player").transform.position = GameObject.Find("OfficePosition").transform.position;
        lobbyTimer = 0.1f;
    }

    //Saving Process
    
    public object SaveState()
    {
        return new SaveData()
        {
            ticket = this.haveTicket,
            map = this.haveMap,
            parttwo = this.secondPart,
            endtrain = this.requestTrain,
            playTime = this.gameTime,
            
            playerPosX = GameObject.Find("Player").transform.position.x,
            playerPosY = GameObject.Find("Player").transform.position.y,
            playerPosZ = GameObject.Find("Player").transform.position.z,
            playerRotY = GameObject.Find("Player").transform.eulerAngles.y,
            camRotX = GameObject.Find("Player").GetComponent<FirstPersonController>().rotationX,

            //Achievements
            wayClimber = this.wayClimberState,
            trainDeath = this.trainDeathState,
            allSecrets = this.allSecretsState,
            interactEveryone = this.interactEveryoneState,
            playDog = this.playDogState,
            gameUnder = this.gameUnderState,
            impressGirl = this.impressGirlState,
            bikeAir = this.bikeAirState,
            escapeBabbdi = this.escapeBabbdiState,
            endticket = this.ticketState,

            //Secrets
            secretsFound = this.secretsFound,
            secret00  = this.secretState[0] ,
            secret01  = this.secretState[1] ,
            secret02  = this.secretState[2] ,
            secret03  = this.secretState[3] ,
            secret04  = this.secretState[4] ,
            secret05  = this.secretState[5] ,
            secret06  = this.secretState[6] ,
            secret07  = this.secretState[7] ,
            secret08  = this.secretState[8] ,
            secret09  = this.secretState[9] ,
            secret10 = this.secretState[10],
            secret11 = this.secretState[11],
            secret12 = this.secretState[12],
            secret13 = this.secretState[13],
            secret14 = this.secretState[14],
            secret15 = this.secretState[15],
            secret16 = this.secretState[16],
            secret17 = this.secretState[17],
            secret18 = this.secretState[18],
            secret19 = this.secretState[19],
            secret20 = this.secretState[20],

            //Object in hands
            club = this.club,
            climber = this.climber,
            propeller = this.propeller,
            blower = this.blower,
            flashlight = this.flashlight,
            soap = this.soap,
            ball = this.ball,
            bigball = this.bigball,
            stick = this.stick,
            grabber = this.grabber,
            motorBike = this.motorBike,
            compass = this.compass,
            trumpet = this.trumpet,
            secretfinder = this.secretfinder
        };
    }

    public void ResetSave()
    {
        haveTicket = 0;
        haveMap = 0;
        secondPart = 0;
        requestTrain = 0;
        gameTime = 0;
        GameObject.Find("Player").transform.position = new Vector3(160.193f, 42.1766f, -38.49f);
        GameObject.Find("Player").transform.rotation = Quaternion.Euler(0,0,0);
        GameObject.Find("Player").GetComponent<FirstPersonController>().rotationX = 0;
        wayClimberState = 0;
        trainDeathState = 0;
        allSecretsState = 0;
        interactEveryoneState = 0;
        playDogState = 0;
        gameUnderState = 0;
        impressGirlState = 0;
        bikeAirState = 0;
        escapeBabbdiState = 0;
        ticketState = 0;

        for (int i = 0; i < secretState.Length; i++)
        {
            secretState[i] = 0;
        }
        secretsFound = 0;

        //Object in hands
        club = 0;
        climber = 0;
        propeller = 0;
        blower = 0;
        flashlight = 0;
        soap = 0;
        ball = 0;
        bigball = 0;
        stick = 0;
        grabber = 0;
        motorBike = 0;
        compass = 0;
        trumpet = 0;
        secretfinder = 0;
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;

        haveTicket = saveData.ticket;
        haveMap = saveData.map;
        secondPart = saveData.parttwo;
        requestTrain = saveData.endtrain;
        gameTime = saveData.playTime;

        GameObject.Find("Player").transform.position = new Vector3(saveData.playerPosX, saveData.playerPosY, saveData.playerPosZ);
        GameObject.Find("Player").transform.rotation = Quaternion.Euler(0, saveData.playerRotY, 0);
        GameObject.Find("Player").GetComponent<FirstPersonController>().rotationX = saveData.camRotX;

        //Achievements
        wayClimberState = saveData.wayClimber;
        if (wayClimberState == 1) wayClimber = true;

        wayClimberState = saveData.trainDeath;
        if (wayClimberState == 1) trainDeath = true;

        allSecretsState = saveData.allSecrets;
        if (allSecretsState == 1) allSecrets = true;

        interactEveryoneState = saveData.interactEveryone;
        if (interactEveryoneState == 1) interactEveryone = true;

        playDogState = saveData.playDog;
        if (playDogState == 1) playDog = true;

        gameUnderState = saveData.gameUnder;
        if (gameUnderState == 1) gameUnder = true;

        impressGirlState = saveData.impressGirl;
        if (impressGirlState == 1) impressGirl = true;

        bikeAirState = saveData.bikeAir;
        if (bikeAirState == 1) bikeAir = true;

        escapeBabbdiState = saveData.escapeBabbdi;
        if (escapeBabbdiState == 1) escapeBabbdi = true;

        ticketState = saveData.endticket;
        if (ticketState == 1) ticket = true;
        
        //Secrets
        secretsFound = saveData.secretsFound;
        this.secretState[0] = saveData.secret00 ;
        this.secretState[1] = saveData.secret01 ;
        this.secretState[2] = saveData.secret02 ;
        this.secretState[3] = saveData.secret03 ;
        this.secretState[4] = saveData.secret04 ;
        this.secretState[5] = saveData.secret05 ;
        this.secretState[6] = saveData.secret06 ;
        this.secretState[7] = saveData.secret07 ;
        this.secretState[8] = saveData.secret08 ;
        this.secretState[9] = saveData.secret09 ;
        this.secretState[10] = saveData.secret10;
        this.secretState[11] = saveData.secret11;
        this.secretState[12] = saveData.secret12;
        this.secretState[13] = saveData.secret13;
        this.secretState[14] = saveData.secret14;
        this.secretState[15] = saveData.secret15;
        this.secretState[16] = saveData.secret16;
        this.secretState[17] = saveData.secret17;
        this.secretState[18] = saveData.secret18;
        this.secretState[19] = saveData.secret19;
        this.secretState[20] = saveData.secret20;

        //Object in hands
        club = saveData.club;
        climber = saveData.climber;
        propeller = saveData.propeller;
        blower = saveData.blower;
        flashlight = saveData.flashlight;
        soap = saveData.soap;
        ball = saveData.ball;
        bigball = saveData.bigball;
        stick = saveData.stick;
        grabber = saveData.grabber;
        motorBike = saveData.motorBike;
        compass = saveData.compass;
        trumpet = saveData.trumpet;
        secretfinder = saveData.secretfinder;
    
    }

    [Serializable]
    private struct SaveData
    {
        public int ticket;
        public int map;
        public int parttwo;
        public int endtrain;
        public float playTime;

        public float playerPosX;
        public float playerPosY;
        public float playerPosZ;
        public float playerRotY;
        public float camRotX;

        //Achievements
        public int wayClimber;
        public int trainDeath;
        public int allSecrets;
        public int interactEveryone;
        public int playDog;
        public int gameUnder;
        public int impressGirl;
        public int bikeAir;
        public int escapeBabbdi;
        public int endticket;

        //Secrets
        public int secretsFound;
        public int secret00;
        public int secret01;
        public int secret02;
        public int secret03;
        public int secret04;
        public int secret05;
        public int secret06;
        public int secret07;
        public int secret08;
        public int secret09;
        public int secret10;
        public int secret11;
        public int secret12;
        public int secret13;
        public int secret14;
        public int secret15;
        public int secret16;
        public int secret17;
        public int secret18;
        public int secret19;
        public int secret20;

        //Object in hands
        public int club;
        public int climber;
        public int propeller;
        public int blower;
        public int flashlight;
        public int soap;
        public int ball;
        public int bigball;
        public int stick;
        public int grabber;
        public int motorBike;
        public int compass;
        public int trumpet;
        public int secretfinder;

    }
}
