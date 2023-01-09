using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    
    public TextMeshProUGUI textBox;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = GameObject.Find("GameManager").GetComponent<GameManager>().gameTime.ToString("F2");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        textBox.text = gameManager.gameTime.ToString("F2");
    }
}
