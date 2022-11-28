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

    private bool fly;
    private bool walk;

    private Vector3 initPos;
    private Vector3 flyDir;

    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        flyTimer -= Time.deltaTime;

        if (distanceFromPlayer < maxDistance)
        {
            if (!fly)
            {
                flyDir = (GameObject.Find("Player").transform.position - transform.position).normalized;
                fly = true;
                flyTimer = flyTime;
            }

        }

        if (fly)
        {
            transform.position += new Vector3(flyDir.x * flySpeed, verticalMovement, flyDir.z * flySpeed);
            //if (transform.position == initPos) fly = false;
            
        }

        if (flyTimer < 0)
        {
            fly = false;
            transform.position = initPos;
        }

        
    }
}
