using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    [SerializeField] AudioClip selectClip;

    public void assignIndex()
    {
        menuButtonController.index = thisIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);

            if (Input.GetButtonDown("Submit"))
            {
                animator.SetBool("pressed", true);
                animatorFunctions.PlaySound(selectClip);
            }
            else if (animator.GetBool("pressed"))
            {
                
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }
}
