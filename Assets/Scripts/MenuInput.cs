using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    [SerializeField] private AudioClip[] jingle;
    [SerializeField] private AudioClip press;
    public void OnPress()
    {
        SoundManager.Instance.PlaySound(jingle[Random.Range(0, jingle.Length - 1)]);
    }

    public void OnClickDown()
    {
        SoundManager.Instance.PlaySound(press);
    }
}
