using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoTicketPopup : MonoBehaviour
{
    private float timer = -5;
    [SerializeField] private TMP_Text textLabel;
    private Color c;
    // Start is called before the first frame update
    void Awake()
    {
        c = textLabel.color;
        c.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textLabel.color = c;

        timer -= Time.deltaTime;
        if (GameObject.Find("GameManager").GetComponent<GameManager>().noticketPopup == true)
        {
            c.a = 1f;
            timer = 3f;
            GameObject.Find("GameManager").GetComponent<GameManager>().noticketPopup = false;
        }
        if (timer < 0 && timer > -5) 
        {
            if (c.a >= 0) c.a -= 0.01f;
        }
    }
        
}
