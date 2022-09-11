using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private Sprite defaultImg;
    [SerializeField] private Sprite interactImg;
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        defaultImg = GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.GetComponent<FirstPersonController>().currentInteractable != null && player.GetComponent<FirstPersonController>().interactionRay))
        {
            GetComponent<Image>().sprite = interactImg;
            GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        }
        else
        {
            GetComponent<Image>().sprite = defaultImg;
            GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        }
           

    }
}
