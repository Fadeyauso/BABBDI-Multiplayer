using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : Interactable
{
    private float mouseX;
    private float mouseY;
    private float timer;
    private bool active;
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
            var magnitude = new Vector2(mouseX, mouseY).magnitude;
            if (Input.GetButton("Fire1") && magnitude > 0.3f && timer < 0)
            {
                timer = 0.2f;
                SoundManager.Instance.PlaySound(pianoSound);
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                SoundManager.Instance.PlaySound(pianoSound);
            }
        }
    }
}
