using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int gunId;
    [Header("Values")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    private float nextTimeToFire = 0f;

    private Vector3 spread = new Vector3(0,0,0);

    public int maxAmmo = 16;
    public int currentAmmo;
    public int ammoStock;
    public float reloadTime = 0.8f;
    private bool isReloading = false;

    public Animator animator;

    public Camera fpsCam;

    [Header("SFX")]
    public AudioClip shoot;
    public AudioClip impact;
    public AudioClip reload;
    public AudioClip woodImpact;
    public AudioClip stoneImpact;
    public AudioClip grassImpact;

    [Header("VFX")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject woodEffect;
    public GameObject stoneEffect;
    public GameObject grassEffect;
    public GameObject bloodEffect;
    public GameObject enemyDeadEffect;

    [Header("Hole")]
    public GameObject impactHole;
    public GameObject woodHole;
    public GameObject stoneHole;
    public GameObject grassHole;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    void OnEnable()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Reloading", isReloading);
        GameObject.Find("AmmoStock").GetComponent<TMPro.TextMeshProUGUI>().text = ammoStock.ToString();
        GameObject.Find("CurrentAmmo").GetComponent<TMPro.TextMeshProUGUI>().text = currentAmmo.ToString();

   

        if (isReloading)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo && ammoStock > 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (currentAmmo <= 0 && currentAmmo != maxAmmo && ammoStock > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && currentAmmo > 0) 
        {
            Shoot();
            nextTimeToFire = Time.time + 1f/fireRate;
        }
        else if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }

        

        //Dynamically change impact vfx
        RaycastHit impactHit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out impactHit, range))
        {
            if (impactHit.transform.tag == "Footsteps/Wood" || impactHit.transform.tag == "Object/Wood") 
            {
                impactEffect = woodEffect;
                impact = woodImpact;
                impactHole = woodHole;
            }
            if (impactHit.transform.tag == "Footsteps/Stone" || impactHit.transform.tag == "Object/Stone")
            {
                impactEffect = stoneEffect;
                impact = stoneImpact;
                impactHole = stoneHole;
            }
            if (impactHit.transform.tag == "Footsteps/Grass")
            {
                impactEffect = grassEffect;
                impact = grassImpact;
                impactHole = grassHole;
            }
            if (impactHit.transform.tag == "Enemy")
            {
                impactEffect = bloodEffect;
                impact = stoneImpact;
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        SoundManager.Instance.PlaySound(reload);
        yield return new WaitForSeconds(reloadTime);
        if (ammoStock - maxAmmo >= 0) 
        {
            ammoStock -= maxAmmo - currentAmmo;
            currentAmmo = maxAmmo;
        }
        else {
            currentAmmo = ammoStock;
            ammoStock = 0;
        }
        isReloading = false;
    }

    void Shoot()
    {
        muzzleFlash.Play();
        SoundManager.Instance.PlaySound(shoot);

        currentAmmo--;

        RaycastHit hit;
        
        if (gunId == 0) spread = fpsCam.transform.forward;
        if (gunId == 1) spread = fpsCam.transform.forward;
        if (gunId == 2) spread = fpsCam.transform.forward + new Vector3(Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f));

        if (Physics.Raycast(fpsCam.transform.position, spread, out hit, range) && hit.collider.tag != "Zone")
        {
            Debug.Log(hit.transform.tag);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
                Instantiate(enemyDeadEffect, hit.transform.position, Quaternion.LookRotation(hit.normal));
            }

            if (hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * impactForce);

            SoundManager.Instance.PlaySound(impact);
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            if (hit.transform.tag == "Object/Wood" || hit.transform.tag == "Object/Stone") Instantiate(impactHole, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            else if (hit.transform.tag != "Enemy") Instantiate(impactHole, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
