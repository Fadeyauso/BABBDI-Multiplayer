using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Sprite bigMap;
    [SerializeField] private Sprite smallMap;
    [SerializeField] private GameObject mapButton;
    public bool mapBig = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !mapButton.GetComponent<MapButton>().mouseOver) mapBig = !mapBig;

        if (mapBig) GetComponent<Image>().sprite = bigMap;
        else if (!mapBig) GetComponent<Image>().sprite = smallMap;
    }
}
