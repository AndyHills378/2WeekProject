using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private MenuButtonController menuButtonController;

    public bool disableOnce;

    public void PlaySound(AudioClip sound)
    {
        if(!disableOnce)
        {
            menuButtonController.audioSource.PlayOneShot(sound);
        }
        else
        {
            disableOnce = false;
        }
    }
}
