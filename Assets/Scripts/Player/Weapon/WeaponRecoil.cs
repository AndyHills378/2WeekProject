using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    private MouseLook recoilCamera;
    public float verticalRecoil;

    private void Start()
    {
        recoilCamera = playerCamera.GetComponent<MouseLook>();
    }

    public void GenerateRecoil()
    {
        recoilCamera.recoil += verticalRecoil;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
