using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climber : MonoBehaviour
{
    private GameObject player;
    float timer = 0;
    [SerializeField] private float launchForce;
    [SerializeField] private float climberForce;
    private float mousey;

    public AudioClip hold;
    public AudioClip hit;

    public Vector3 pickPos;
    public Quaternion pickRot;

    public bool trigger;
    public bool pick = false;

    public bool touch;

    void Awake(){
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [KNetMessageHandler(typeof(OnItemUsedMessage))]
    public void OnPickaxeUsed(OnItemUsedMessage message)
    {
        Debug.Log("Pickaxe used");
    }
    // Update is called once per frame
    void Update()
    {
        mousey = -Mathf.Clamp(Input.GetAxis("Mouse Y"), -2, 2);

        timer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump") && trigger)
        {

            pick = false;
            timer = 0.2f;
            trigger = false;
            player.GetComponent<FirstPersonController>().AddForce((player.transform.right / 15) * player.GetComponent<FirstPersonController>().currentInputRaw.y, launchForce);
            player.GetComponent<FirstPersonController>().moveDirection.y = player.GetComponent<FirstPersonController>().jumpForce * climberForce * (mousey > 1 ? mousey : 1);
        }

        if (!Input.GetButton("Fire1") && !player.GetComponent<FirstPersonController>().pause)
        {
            trigger = false;
        }

        if (Input.GetButtonDown("Fire1") && !player.GetComponent<FirstPersonController>().pause)
        {
            timer = 0f;
            hitb = true;
            pick = true;

        }

        if (GetComponent<InteractObject>().extended && timer > 0 && timer < 0.05f)
        {
            hitb = true;
        }
        
    }
    bool hitb;
    void OnTriggerEnter(Collider collisionInfo)
    {
        touch = true;
        pickPos = transform.position;
                pickRot = transform.rotation;
        
    }

    void OnTriggerStay(Collider collisionInfo)
    {

        if (collisionInfo.gameObject.layer == 7 && GetComponent<InteractObject>().extended && pick || collisionInfo.gameObject.layer == 0 && GetComponent<InteractObject>().extended && pick)
        {
            
            touch = false;
            trigger = true;

            if (hitb) 
            {
                SoundManager.Instance.PlaySound(hit);
                hitb = false;
                pickPos = GameObject.Find("Pickaxe00").transform.position;
                pickRot = GameObject.Find("Pickaxe00").transform.rotation;
            }
            

            if (player.GetComponent<FirstPersonController>().climbRay) 
            {
                
            }
        }

    }

}
