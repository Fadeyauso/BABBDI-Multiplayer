using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeFirstSelectedButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeButton(GameObject button)
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(button, new BaseEventData(eventSystem));
    }
}
