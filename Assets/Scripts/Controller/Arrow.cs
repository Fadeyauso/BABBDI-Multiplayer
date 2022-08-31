using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public GameObject parent;
    public GameObject defaultPos;
    private Rigidbody rb;
    public Vector3 newVector;
    private bool hit;
    [SerializeField] private float impulsionForce;
    [SerializeField] private float horizontalForce;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float deceleration;

    public bool onWall;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        parent = transform.parent.gameObject;
        defaultPos = parent.GetComponent<Grabber>().arrowInitPosition;
    }

    // Update is called once per frame
    void Update()
    {

        if (parent.GetComponent<InteractObject>().inHands)
        {
            if (transform.parent != null) 
            {
                parent = transform.parent.gameObject;
                defaultPos = parent.GetComponent<Grabber>().arrowInitPosition;
                transform.parent = parent.transform;
                rb.isKinematic = true;
                transform.position = defaultPos.transform.position;
                if (Input.GetButtonDown("Fire1"))
                {
                    hit = true;
                    transform.parent = null;
                    rb.isKinematic = false;
                    rb.AddForce(GameObject.Find("Player").GetComponent<FirstPersonController>().playerCamera.transform.forward * arrowSpeed);
                }
            }
            else if (transform.parent == null)
            {
                rb.isKinematic = false;
                if (Input.GetButtonDown("Fire1"))
                {
                    rb.constraints = 0;
                    transform.position = defaultPos.transform.position;
                    transform.parent = parent.transform;
                    onWall = false;
                }
            }
        }

        
        
    }

    void OnCollisionStay(Collision collision)
    {
        if (transform.parent == null && parent.GetComponent<InteractObject>().inHands && hit)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            newVector = (GameObject.Find("Player").transform.position - transform.position).normalized;
            onWall = true;
            hit = false;
            GameObject.Find("Player").GetComponent<FirstPersonController>().AddVerticalForce(new Vector3(0, -newVector.y, 0), impulsionForce);
            GameObject.Find("Player").GetComponent<FirstPersonController>().AddHorizontalForce(new Vector3(-newVector.x, 0, -newVector.z), impulsionForce);
            GameObject.Find("Player").GetComponent<FirstPersonController>().deceleration = deceleration;
        }
        
    }
}
