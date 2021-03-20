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

    private float aimDuration = .3f;
    RaycastWeapon weapon;
    private bool weaponExists;
    GameObject arTempCamera;

    void Start()
    {
        //arTempCamera = Instantiate(mainCamera, mainCamera.transform.position, mainCamera.transform.rotation);
        //arTempCamera.SetActive(false);
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
                    //mainCamera.SetActive(false);
                    //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, ARCameraPosition.transform.position, .8f);
                }
                else
                {
                    aimLayer.weight -= Time.deltaTime / aimDuration;
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCameraOriginal.transform.position, .8f);
                    //Destroy(arTempCamera);
                    //mainCamera.transform.position = Vector3.Lerp(ARCameraPosition.transform.position, startingPosition, .8f);
                }
                if (Input.GetButton("Fire1"))
                {
                    weapon.StartFiring();
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    weapon.StopFiring();
                }
            }
        }
    }
}
