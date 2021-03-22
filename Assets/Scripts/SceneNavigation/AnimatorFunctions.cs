using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private MenuButtonController menuButtonController;
    [SerializeField] private AudioSource controller;

    public bool disableOnce;

    public void PlaySound(AudioClip sound)
    {
        if(!disableOnce)
        {
            if (menuButtonController)
            {
                menuButtonController.audioSource.PlayOneShot(sound);
            }
            else if (controller)
            {
                controller.PlayOneShot(sound);
            }
        }
        else
        {
            disableOnce = false;
        }
    }
}
