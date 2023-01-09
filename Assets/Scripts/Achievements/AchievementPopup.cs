using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementPopup : MonoBehaviour
{
    [SerializeField] private GameObject initPos;
    [SerializeField] private GameObject upPos;

    [SerializeField] private TMP_Text textLabel;

    private float timer = 0f;
    private Vector3 buttonVelocity = Vector3.zero;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        textLabel.text = gameManager.lastAchievement;
        if (gameManager.popup)
        {
            gameManager.popup = false;
            timer = 10f;
            
        }

        if (timer > 0)
        {
            if (transform.position.y < upPos.transform.position.y) transform.position += new Vector3(0,130,0) * Time.deltaTime;
        }

        if (timer < 0)
        {
            if (transform.position.y > initPos.transform.position.y) transform.position -= new Vector3(0,130,0) * Time.deltaTime;
        }
    }

    
}
