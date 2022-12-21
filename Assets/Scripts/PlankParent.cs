using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankParent : MonoBehaviour
{
    public bool destroyed;
    public bool playSound = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playSound && destroyed)
        {
            SoundManager.Instance.PlaySound(GameObject.Find("GameManager").GetComponent<GameManager>().destroy);
            playSound = false;
        }
    }
}
