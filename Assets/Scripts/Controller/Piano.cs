using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : Interactable
{
    private float mouseX;
    private float mouseY;
    private float timer;
    private bool active;

    private Vector3 initPos;
    private Vector3 pressPos;
    void Awake()
    {
        initPos = transform.position;
        pressPos = new Vector3(initPos.x, initPos.y-0.05f, initPos.z);
    }

    [SerializeField] private AudioClip pianoSound;
    public override void OnFocus()
    {
        active = true;
    }
    public override void OnInteract()
    {
        
        
    }

    public override void OnLoseFocus()
    {
        active = false;
        transform.position = Vector3.Lerp(transform.position, initPos, 12f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad)
        {
            mouseX = Input.GetAxis("CameraHorizontal");
            mouseY = Input.GetAxis("CameraVertical");
        }
        else{
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        if (GameObject.Find("Player").GetComponent<FirstPersonController>().currentObject == this.gameObject && active)
        {
            if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F)) transform.position = Vector3.Lerp(transform.position, pressPos, 12f * Time.deltaTime);
            else transform.position = Vector3.Lerp(transform.position, initPos, 12f * Time.deltaTime);

            var magnitude = new Vector2(mouseX, mouseY).magnitude;
            if ((Input.GetButton("Fire1") || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F)) && magnitude > 0.3f && timer < 0)
            {
                timer = 0.2f;
                SoundManager.Instance.PlaySound(pianoSound);
                
            }
            else if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
            {
                SoundManager.Instance.PlaySound(pianoSound);
            }
        }
        if (GameObject.Find("Player").GetComponent<FirstPersonController>().currentObject != this.gameObject) transform.position = Vector3.Lerp(transform.position, initPos, 12f * Time.deltaTime);
    }
}
