using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private string normalText;
    [SerializeField] private string gamepadText;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad) textLabel.text = gamepadText;
        else textLabel.text = normalText;
    }
}
