using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Trumpet : MonoBehaviour
{
    private bool active;
    private AudioSource audio;

    public int scaleIndex;
    public float[] scaleFactor;
    public float[] scaleDecal;
    public float[] scaleLimit;
    [SerializeField] private GameObject scalePopup;
    [SerializeField] private string[] scaleName;

    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        scalePopup = GameObject.Find("ScalePopup");
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<InteractObject>().inHands && !GameObject.Find("Player").GetComponent<FirstPersonController>().pause)
        
        {
            if (Input.GetButtonDown("Fire1"))
            {
                audio.Play();
                active = true;
            }

            if (Input.GetButtonUp("Fire1") || Input.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
            {
                audio.Stop();
                active = false;
            }

            audio.pitch = Mathf.Round((-GameObject.Find("Player").GetComponent<FirstPersonController>().rotationX / 90f * scaleLimit[scaleIndex]) * scaleFactor[scaleIndex]) / scaleFactor[scaleIndex] + scaleDecal[scaleIndex];

            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetButtonDown("Slide"))
            {
                if (scaleIndex >= scaleFactor.Length - 1)
                    scaleIndex = 0;
                else 
                    scaleIndex++;

                scalePopup.GetComponent<ScalePopup>().textLabel.text = scaleName[scaleIndex];

                scalePopup.GetComponent<ScalePopup>().c.a = 2f;
                scalePopup.GetComponent<ScalePopup>().c1.a = 1f;
                scalePopup.GetComponent<ScalePopup>().timer = 0.7f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetButtonDown("Slide"))
            {
                if (scaleIndex <= 0)
                    scaleIndex = scaleFactor.Length - 1;
                else 
                    scaleIndex--;

                scalePopup.GetComponent<ScalePopup>().textLabel.text = scaleName[scaleIndex];

                scalePopup.GetComponent<ScalePopup>().c.a = 2f;
                scalePopup.GetComponent<ScalePopup>().c1.a = 1f;
                scalePopup.GetComponent<ScalePopup>().timer = 0.7f;
            }
        }


    }
}
