using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamepadPopup : MonoBehaviour
{
    private float timer = -5;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private Image textLabel1;
    private Color c;
    private Color c1;
    // Start is called before the first frame update
    void Awake()
    {
        c = textLabel.color;
        c1 = textLabel1.color;
        c.a = 0;
        c1.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textLabel.color = c;
        textLabel1.color = c1;

        timer -= Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q)) && GameObject.Find("GameManager").GetComponent<GameManager>().gamepad && !GameObject.Find("Player").GetComponent<FirstPersonController>().pause)
        {
            c.a = 1f;
            c1.a = 1f;
            timer = 0.7f;
        }
        if (timer < 0) 
        {
            if (c.a >= 0) c.a -= 2f * Time.deltaTime;
            if (c1.a >= 0) c1.a -= 2f * Time.deltaTime;
        }
    }
}
