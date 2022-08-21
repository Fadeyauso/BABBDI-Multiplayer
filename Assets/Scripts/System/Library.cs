using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Library : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "You collected " + GameManager.secretsFound + "/24 secrets.";
    }
}
