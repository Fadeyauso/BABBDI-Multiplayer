using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrain : MonoBehaviour
{
    [SerializeField] private Transform endPos;
    [SerializeField] private float trainSpeed;
    [SerializeField] private float accel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().endGame)
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0.1f, 1f * Time.deltaTime);
            GameObject.Find("AmbientSource").GetComponent<AudioSource>().mute = true;
            transform.position = new Vector3(transform.position.x - 10 * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().requestTrain == 1)
        {
            transform.position = Vector3.Lerp(transform.position, endPos.transform.position, trainSpeed * Time.deltaTime);
        }
    }
}
