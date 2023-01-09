using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Library : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    // Start is called before the first frame update
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "You collected " + gameManager.secretsFound + "/21 secret objects.";
    }
}
