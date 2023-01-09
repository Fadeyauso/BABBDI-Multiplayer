using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Outro : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color tempColor;
    [SerializeField] private float fadeSpeed;
    public TMP_Text runTime;
    public GameObject title;
    public GameObject text;
    public GameObject text2;
    public GameObject nextState;

    public float timer = 3;
    float initTimer;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Awake()
    {
        //image = GetComponent<Image>();
    }

    void Start()
    {
        tempColor = image.color;
        initTimer = timer;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().endGame && timer > -4f)
        {
            timer -= Time.deltaTime;
            if (timer < 5)
            {
                tempColor.a += Time.deltaTime * fadeSpeed;
                image.color = tempColor;
            }
            if (timer < -1f)
            {
                title.SetActive(true);
            }   
            if (timer < -2)
            {
                text.SetActive(true);
            }
            if (timer < -3f)
            {
                text2.SetActive(true);
            }
            if (timer < -4f)
            {
                nextState.SetActive(true);
            }

            runTime.text = "You completed the game in " +  GameObject.Find("GameManager").GetComponent<GameManager>().gameTime.ToString() + " seconds.";


        }
        
        
    }
}
