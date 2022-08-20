using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemActivator : Interactable
{
    public GameObject objectToActivate;
    public AudioClip activateClip;
    public Animator anim;
    private bool trigger;

    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (objectToActivate != null)
        {
            if (objectToActivate.GetComponent<Lift>() != null && objectToActivate.GetComponent<Lift>().isTriggered == false) objectToActivate.GetComponent<Lift>().isTriggered = true;
            SoundManager.Instance.PlaySound(activateClip);
            trigger = !trigger;
        }

        anim.SetBool("trigger", trigger);
    }

    public override void OnLoseFocus()
    {


    }
}
