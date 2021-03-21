using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private MenuButtonControler menuButtonControler;

    public bool disableOnce;

    void PlaySound(AudioClip sound)
    {
        if(!disableOnce)
        {
            menuButtonControler.audioSource.PlayOneShot(sound);
        }
        else
        {
            disableOnce = false;
        }
    }
}
