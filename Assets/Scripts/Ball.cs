using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private AudioClip ball; 
    [SerializeField] private AudioSource ballSound; 

    private float timer;
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
    }

    void OnCollisionEnter(Collision collision)
    {  
        if (collision.collider.tag == "Dog")
        {
            if (!gameManager.playDog) gameManager.Popup();
            gameManager.playDog = true;
            gameManager.playDogState = 1;
            gameManager.lastAchievement = "Doggo friendly";
            SteamIntegration.Instance.UnlockAchivement("DoggoFriendly");
        }

        

        if (GetComponent<Rigidbody>().velocity.magnitude > 2) ballSound.PlayOneShot(ball);
        timer = 0.8f;
    }

    void OnTriggerEnter(Collider col)

    {
        if ((col.gameObject.layer == 16))
        {
            GetComponent<Rigidbody>().AddForce(GameObject.Find("Player").GetComponent<FirstPersonController>().playerCamera.transform.forward * 4, ForceMode.Impulse);
            
        }
    }


}
