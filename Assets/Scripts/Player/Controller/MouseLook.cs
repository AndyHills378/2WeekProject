using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

// TODO: Disable on 2d Minigames

public class MouseLook : NetworkBehaviour
{

    //[Header("Networking")]
    //[SerializeField] private Camera mainCamera = null;

    [SerializeField] private float minRot;
    [SerializeField] private float maxRot;
    public float mouseSensitivity;

    public Transform playerBody;
    
    public float xRotation = 0f;
    public float recoil;

    /*public override void OnStartAuthority()
    {
        mainCamera.gameObject.SetActive(true);

        enabled = true;
    }

    [ClientCallback]*/
    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY + recoil;
        xRotation = Mathf.Clamp(xRotation, minRot, maxRot);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * MouseX);
    }
}
