using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponRecoil : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerCamera;

    private MouseLook recoilCamera;
    public float verticalRecoil = 0;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [Client]
    private void Start()
    {
        recoilCamera = playerCamera.GetComponent<MouseLook>();
    }

    [Client]
    public void GenerateRecoil()
    {
        recoilCamera.recoil += verticalRecoil;
    }

    [Client]
    public void StopRecoil()
    {
        recoilCamera.recoil = 0;
    }
}
