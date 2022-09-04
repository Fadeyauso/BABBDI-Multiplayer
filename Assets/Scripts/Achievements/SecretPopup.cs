using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecretPopup : MonoBehaviour
{
    private float timer = -5;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text textLabel1;
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
        if (GameObject.Find("GameManager").GetComponent<GameManager>().secretPopup == true)
        {
            c.a = 1f;
            c1.a = 1f;
            timer = 3f;
            GameObject.Find("GameManager").GetComponent<GameManager>().secretPopup = false;
        }
        if (timer < 0 && timer > -5) 
        {
            if (c.a >= 0) c.a -= 0.01f;
            if (c1.a >= 0) c1.a -= 0.01f;
        }
    }


}
