using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExclamationPoint : MonoBehaviour
{
    public float glowTimer;
    public float glowSpeed;
    public float glowAmount;
    private Vector3 initScale;
    public AudioClip initClip;
    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        glowTimer += Time.deltaTime * glowSpeed;

        transform.localScale = initScale * Mathf.Sin(glowTimer) * glowAmount;
    }
}