using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] private Rig aimLayer;
    private float aimDuration = .3f;
    RaycastWeapon weapon;
    private bool weaponExists;

    void Start()
    {
        weapon = GetComponentInChildren<RaycastWeapon>();
        if(weapon != null)
        {
            weaponExists = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponExists)
        {
            weapon.UpdateBullet(Time.deltaTime);
            if (aimLayer)
            {
                if (Input.GetButton("Fire2"))
                {
                    aimLayer.weight += Time.deltaTime / aimDuration;
                    if (Input.GetButton("Fire1"))
                    {
                        weapon.StartFiring();
                    }
                    if (Input.GetButtonUp("Fire1"))
                    {
                        weapon.StopFiring();
                    }
                }
                else
                {
                    aimLayer.weight -= Time.deltaTime / aimDuration;
                    weapon.StopFiring();
                }
            }
        }

    }
}
