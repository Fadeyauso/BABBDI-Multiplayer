using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathAnim : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color black;
    [SerializeField] private Color tempColor;
    [SerializeField] private float fadeSpeed;

    public float timer = 0;


    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
         if (timer < 0f)
        {
            tempColor.a -= Time.deltaTime * fadeSpeed * 6;
            image.color = tempColor;
        }

        
    }

    public void Trigger()
    {
        timer = 1f;
        image.color = black;
        tempColor = black;
    }
}
