using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Dog")
        {
            if (!GameObject.Find("GameManager").GetComponent<GameManager>().playDog) GameObject.Find("GameManager").GetComponent<GameManager>().Popup();
            GameObject.Find("GameManager").GetComponent<GameManager>().playDog = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().playDogState = 1;
            GameObject.Find("GameManager").GetComponent<GameManager>().lastAchievement = "Doggo friendly";
        }
    }
}
