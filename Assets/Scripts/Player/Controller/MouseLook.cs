using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class MouseLook : NetworkBehaviour
{
    [Header("Networking")]
    [HideInInspector] public Transform playerBody;

    [Header("Camera")]
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private float minRot;
    [SerializeField] private float maxRot;
    public float mouseSensitivity;
    private float xRotation = 0f;

    [HideInInspector] public float recoil = 0;

    [Client]
    private void CmdMovement()
    {
        if (playerBody != null)
        {
            float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= MouseY + recoil;
            xRotation = Mathf.Clamp(xRotation, minRot, maxRot);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * MouseX);
        }
    }

    [ClientCallback]
    void Update()
    {
        if(playerBody.GetComponent<PlayerMovement>().hasAuthority)
        {
            enabled = true;
            this.GetComponent<Camera>().enabled = true;
            this.GetComponent<AudioListener>().enabled = true;
            miniMapCamera.GetComponent<Camera>().enabled = true;
            CmdMovement();
        }
    }
}
