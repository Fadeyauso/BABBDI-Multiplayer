using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private bool isOpen = false;
    private bool canBeInteractedWith = true;
    private Animator anim;

    public bool canClose = true;

    public AudioClip openDoorClip;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.layer = 11;
    }

    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (canBeInteractedWith && canClose)
        {
            if (!isOpen) canClose = false;
            isOpen = !isOpen;
            
            Vector3 doorTransformDirection = transform.TransformDirection(Vector3.right);
            Vector3 playerTransformDirection = FirstPersonController.instance.transform.position - transform.position;
            float dot = Vector3.Dot(doorTransformDirection, -playerTransformDirection);

            anim.SetFloat("dot", dot);
            anim.SetBool("isOpen", isOpen);
            
            SoundManager.Instance.PlaySound(openDoorClip);
        }
    }

    public override void OnLoseFocus()
    {

    }


    public void CanClose()
    {
        canClose = true;
    }
}
