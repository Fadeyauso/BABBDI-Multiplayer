using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    using TMPro;

public class ScalePopup : MonoBehaviour
{
    public float timer = -5;
    [SerializeField] public TMP_Text textLabel;
    [SerializeField] private Image textLabel1;
    public Color c;
    public Color c1;
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

        if (timer < 0) 
        {
            if (c.a >= 0) c.a -= 4f * Time.deltaTime;
            if (c1.a >= 0) c1.a -= 2f * Time.deltaTime;
        }
    }
}
