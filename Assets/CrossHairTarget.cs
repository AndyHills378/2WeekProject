using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;

    // Update is called once per frame
    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;

        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;
    }
}
