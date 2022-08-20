using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{


    public float maxHealth = 100f;
    public float health = 100f;
    private float invincibiltyTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        invincibiltyTimer -= Time.deltaTime;
        GameObject.Find("Health").GetComponent<TMPro.TextMeshProUGUI>().text = health.ToString() + " HP";
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Enemy" || collisionInfo.tag == "Projectile") 
        {
            if (invincibiltyTimer < 0) 
            {
                health -= collisionInfo.GetComponent<Target>().damage;
                invincibiltyTimer = 0.5f;
            }

            if (collisionInfo.tag == "Projectile")
            {
                Destroy(collisionInfo.gameObject,0.1f);
            }
        }
    }
}
