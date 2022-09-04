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
    public GameObject text;
    public GameObject nextState;

    public float timer = 0;


    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        tempColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().endGame)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                tempColor.a += Time.deltaTime * fadeSpeed;
                image.color = tempColor;
            }
            if (timer < 3)
            {
                text.SetActive(true);
            }   
            if (timer < 6)
            {
                nextState.SetActive(true);
            }

            runTime.text = "You completed the game in " +  GameObject.Find("GameManager").GetComponent<GameManager>().gameTime.ToString() + " seconds.";


        }
        
        
    }
}
