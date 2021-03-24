using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;

public class CharacterAiming : NetworkBehaviour
{
    [SerializeField] private Rig aimLayer;
    [SerializeField] private GameObject ARCameraPosition;
    [SerializeField] private GameObject mainCameraOriginal;

    private Camera mainCamera;
    private MouseLook weaponRecoil;
    private float aimDuration = .3f;
    RaycastWeapon weapon;
    private bool weaponExists;
    public bool playerAiming;

    [Client]
    private void Aiming()
    {
        if (weaponExists)
        {
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
            }
        }
    }

    [ClientCallback]
    public void Start()
    {
        mainCamera = GetComponent<PlayerMovement>().mainCamera;
        weaponRecoil = GetComponentInChildren<MouseLook>();
        weapon = GetComponentInChildren<RaycastWeapon>();
        if (weapon != null)
        {
            weaponExists = true;
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    void Update()
    {
        Aiming();
    }
}
