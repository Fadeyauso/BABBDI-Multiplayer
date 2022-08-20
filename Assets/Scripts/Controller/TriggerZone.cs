using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{

    public GameObject sun;

    public void Awake()
    {
        sun = GameObject.Find("Sun");
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Zone")
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, collisionInfo.GetComponent<ZoneProperties>().fogColor, 0.01f);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, collisionInfo.GetComponent<ZoneProperties>().fogDensity, 0.01f);
            sun.GetComponent<Light>().color = Color.Lerp(sun.GetComponent<Light>().color, collisionInfo.GetComponent<ZoneProperties>().sunColor, 0.01f);

            RenderSettings.skybox = collisionInfo.GetComponent<ZoneProperties>().sunSource;
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, collisionInfo.GetComponent<ZoneProperties>().intensityMultiplier, 0.01f);
        }
    }
}
