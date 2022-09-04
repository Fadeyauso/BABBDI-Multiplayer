using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    
    public TextMeshProUGUI textBox;
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = GameObject.Find("GameManager").GetComponent<GameManager>().gameTime.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        textBox.text = GameObject.Find("GameManager").GetComponent<GameManager>().gameTime.ToString("F2");
    }
}
