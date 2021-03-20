using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] private Rig aimLayer;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject ARCameraPosition;
    [SerializeField] private GameObject mainCameraOriginal;

    private MouseLook weaponRecoil;
    private float aimDuration = .3f;
    RaycastWeapon weapon;
    private bool weaponExists;
    public bool playerAiming;

    void Start()
    {
        weaponRecoil = GetComponentInChildren<MouseLook>();
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
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, ARCameraPosition.transform.position, .8f);
                    playerAiming = true;
                }
                else
                {
                    aimLayer.weight -= Time.deltaTime / aimDuration;
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCameraOriginal.transform.position, .8f);
                    playerAiming = false;
                }
                if (Input.GetButton("Fire1"))
                {
                    weapon.StartFiring();
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    weaponRecoil.recoil = 0;
                    weapon.StopFiring();
                }
            }
        }
    }
}
