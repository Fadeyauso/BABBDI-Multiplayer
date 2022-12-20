using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarShoutZone : MonoBehaviour
{
    [SerializeField] private AudioSource shoutLeft;
    [SerializeField] private AudioSource shoutRight;

    [SerializeField] private float resetTime = 60;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > resetTime - 1)
        {
            shoutLeft.Play();
            shoutRight.Play();

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (timer < 0)
            {
                timer = resetTime;
            }
        }
    }
}
