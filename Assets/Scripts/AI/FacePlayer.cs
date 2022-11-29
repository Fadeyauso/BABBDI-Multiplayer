using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float maxDistance = 7;
    [SerializeField] private float damping = 3;

    [SerializeField] private float minXRot = -35;
    [SerializeField] private float maxXRot = 35;
    [SerializeField] private float minYRot = -50;
    [SerializeField] private float maxYRot = 35;

    [SerializeField] private bool intervals;
    [SerializeField] private bool weirdNegative;
    [SerializeField] private Vector2 negativeInterval;
    [SerializeField] private Vector2 positiveInterval;

    [SerializeField] private Quaternion defaultRot;
    [SerializeField] private Quaternion facePlayerRot;
    [SerializeField] private Quaternion debug;
    [SerializeField] private Vector3 rotation;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Main Camera");
    }

    void Start()
    {
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        rotation = player.transform.position - transform.position;
        facePlayerRot = Quaternion.LookRotation(rotation);

        debug = Quaternion.Euler(facePlayerRot.eulerAngles.x, facePlayerRot.eulerAngles.y, facePlayerRot.eulerAngles.z);

        if (distanceFromPlayer < maxDistance)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(facePlayerRot.eulerAngles.x, facePlayerRot.eulerAngles.y, facePlayerRot.eulerAngles.z), Time.deltaTime * damping);

            //LimitRot();
        }
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(defaultRot.eulerAngles.x, defaultRot.eulerAngles.y, defaultRot.eulerAngles.z), Time.deltaTime * damping);

    }

    private void LimitRot()
    {
        Vector3 angle = transform.rotation.eulerAngles;

        angle.x = (angle.x > 180) ? angle.x - 360 : angle.x;
        angle.x = Mathf.Clamp(angle.x, minXRot, maxXRot);

        if (intervals){
            angle.y = (angle.y < 0) ? Mathf.Clamp(angle.y, -360, negativeInterval.x) : (angle.y > 0) ? Mathf.Clamp(angle.y, positiveInterval.x, 360) : 180;
        }
        if (weirdNegative){
            angle.y = Mathf.Clamp(angle.y, positiveInterval.x, negativeInterval.x);
        }
        else {
            angle.y = (angle.y > 180) ? angle.y - 360 : angle.y;
            angle.y = Mathf.Clamp(angle.y, minYRot, maxYRot);
        }
        
        transform.rotation = Quaternion.Euler(angle);
    }
}
