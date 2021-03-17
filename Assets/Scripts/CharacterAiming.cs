using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] private Rig aimLayer;
    private float aimDuration = .3f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aimLayer.weight += Time.deltaTime / aimDuration;
        }
        else
        {
            aimLayer.weight -= Time.deltaTime / aimDuration;
        }
    }
}
