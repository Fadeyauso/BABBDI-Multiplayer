using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    public Transform rearRayPos;
    public Transform frontRayPos;
    public LayerMask layerMask;

    public float surfaceAngle;
    public bool uphill;
    public bool downhill;
    public bool flatSurface;


    // Update is called once per frame
    void Update()
    {
        rearRayPos.rotation = Quaternion.Euler(-gameObject.transform.rotation.x, -gameObject.transform.rotation.y, -gameObject.transform.rotation.z);
        frontRayPos.rotation = Quaternion.Euler(-gameObject.transform.rotation.x, -gameObject.transform.rotation.y, -gameObject.transform.rotation.z);

        RaycastHit rearHit;
        if (Physics.Raycast(rearRayPos.position, rearRayPos.TransformDirection(-Vector3.up), out rearHit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rearRayPos.position, rearRayPos.TransformDirection(-Vector3.up) * rearHit.distance, Color.yellow);
            surfaceAngle = Vector3.Angle(rearHit.normal, Vector3.up);
            //Debug.Log(surfaceAngle);
        }
        else
        {
            Debug.DrawRay(rearRayPos.position, rearRayPos.TransformDirection(-Vector3.up) * 1000, Color.red);
            uphill = false;
            //Debug.LogWarning("Downhill");
        }

        RaycastHit frontHit;
        Vector3 frontRayStartPos = new Vector3(frontRayPos.position.x, rearRayPos.position.y, rearRayPos.position.z);
        if (Physics.Raycast(frontRayStartPos, frontRayPos.TransformDirection(-Vector3.up), out frontHit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(frontRayStartPos, frontRayPos.TransformDirection(-Vector3.up) * frontHit.distance, Color.yellow);

        }
        else
        {
            uphill = true;
            //Debug.LogWarning("uphill");
        }
        if(frontHit.distance < rearHit.distance)
        {
            uphill = true;
            downhill = false;
            //Debug.LogWarning("uphill");
        }
        else if (frontHit.distance > rearHit.distance)
        {
            downhill = true;
            uphill = false;
            //Debug.LogWarning("DownHill");
        }
        else if (frontHit.distance == rearHit.distance)
        {
            flatSurface = true;
            uphill = false;
            downhill = false;
            //Debug.LogWarning("Flat surface");
        }

        


    }
}
