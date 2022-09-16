using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemActivator : Interactable
{
    public GameObject objectToActivate;
    public AudioClip activateClip;

    public override void OnFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().elevatorPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        if (objectToActivate != null)
        {
            if (objectToActivate.GetComponent<Lift>() != null && objectToActivate.GetComponent<Lift>().isTriggered == false) 
            {
                objectToActivate.GetComponent<Lift>().isTriggered = true;
                SoundManager.Instance.PlaySound(activateClip);
            }
        }
    }

    public override void OnLoseFocus()
    {

        GameObject.Find("Player").GetComponent<FirstPersonController>().elevatorPopup.SetActive(false);
    }
}
