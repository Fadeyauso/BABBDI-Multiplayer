using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private float impactPower;
    [SerializeField] private AudioClip impactClip;
    public float mouseX;
    public float mouseY;
    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        Debug.Log(new Vector2(mouseX, mouseY).magnitude);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<InteractObject>().inHands && Input.GetButton("Fire1"))
        {
            SoundManager.Instance.PlaySound(impactClip);
            ContactPoint contact = collision.contacts[0];
            player.GetComponent<FirstPersonController>().AddForce(contact.normal, impactPower * Mathf.Clamp(new Vector2(mouseX, mouseY).magnitude, 0, 2));
            
        }
    }
}
