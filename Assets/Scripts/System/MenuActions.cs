using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActions : MonoBehaviour
{
    [SerializeField] private AudioClip pointer;
    [SerializeField] private AudioClip click;

    public void OnPointerEnter()
    {
        SoundManager.Instance.PlaySound(pointer);
        
    }

    public void OnClick()
    {
        SoundManager.Instance.PlaySound(click);
    }

}
