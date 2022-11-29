using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemActivator : Interactable
{
    public GameObject objectToActivate;
    public AudioClip activateClip;
    [SerializeField] private bool upCall;
    [SerializeField] private bool downCall;


    public override void OnFocus()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().elevatorPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        if (objectToActivate != null)
        {
            if (objectToActivate.GetComponent<Lift>() != null && objectToActivate.GetComponent<Lift>().isTriggered == false && upCall && !objectToActivate.GetComponent<Lift>().topReach) 
            {
                objectToActivate.GetComponent<Lift>().isTriggered = true;
                SoundManager.Instance.PlaySound(activateClip);
            }
            else if (objectToActivate.GetComponent<Lift>() != null && objectToActivate.GetComponent<Lift>().isTriggered == false && downCall && objectToActivate.GetComponent<Lift>().topReach) 
            {
                objectToActivate.GetComponent<Lift>().isTriggered = true;
                SoundManager.Instance.PlaySound(activateClip);
            }
            if (objectToActivate.GetComponent<Lift>() != null && objectToActivate.GetComponent<Lift>().isTriggered == false && !upCall && !downCall) 
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
