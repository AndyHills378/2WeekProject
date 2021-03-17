using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO: Disable on 2d Minigames

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity;

    public Transform playerBody;
    
    private float xRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -5f, 26f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * MouseX);


    }
}
