using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float maxDistance = 5;
    [SerializeField] private float flyTime = 20;
    [SerializeField] private float flySpeed = 2;
    [SerializeField] private float verticalMovement = 1;
    [SerializeField] private float flyTimer;
    [SerializeField] private Animator anim;
    private float walkTimer;

    private bool fly;
    private bool walk;

    private Vector3 initPos;
    private Vector3 flyDir;
    private Vector3 rotation;

    private GameObject player;

    [SerializeField] private AudioClip flyClip;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360),0);
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        flyTimer -= Time.deltaTime;

        walkTimer = Mathf.Sin(Time.time) * Random.Range(1,1);

        anim.SetBool("fly", fly);

        

        if (distanceFromPlayer < maxDistance)
        {
            if (!fly)
            {
                SoundManager.Instance.PlaySound(flyClip);
                flyDir = -(GameObject.Find("Player").transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(flyDir);
                fly = true;
                flyTimer = flyTime;
            }

        }

        if (fly)
        {
            transform.position += new Vector3(flyDir.x * Time.deltaTime * flySpeed, verticalMovement * Time.deltaTime, flyDir.z * Time.deltaTime * flySpeed);
            //if (transform.position == initPos) fly = false;
            
        }

        if (flyTimer < 0 && flyTimer > -0.1f)
        {
            fly = false;
            transform.position = initPos;
        }

        /*if (!fly && walkTimer > 0.9f)
        {
            anim.SetBool("walk", true);
        }
        else             anim.SetBool("walk", false);*/


        
    }
}
