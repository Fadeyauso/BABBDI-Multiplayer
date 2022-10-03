using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameFeedback : MonoBehaviour
{
    [SerializeField] private GameObject saveLabel;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        timer = 2f;
    }
}
