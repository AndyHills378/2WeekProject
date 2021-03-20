using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{

    [SerializeField] private Animator rigController;
    [SerializeField] private WeaponAnimationEvents animationEvents;

    private RaycastWeapon weapon;
    public Transform leftHand;
    private GameObject magazineHand;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponentInChildren<RaycastWeapon>();
        animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon)
        {
            if (Input.GetButtonDown("Reload") || weapon.ammoCount <= 0)
            {
                rigController.SetTrigger("reload_weapon");
                weapon.canShoot = false;
                weapon.ammoCount = 0;
            }
        }
    }

    private void OnAnimationEvent(string eventName)
    {
        Debug.Log(eventName);
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attach_magazine":
                AttachMagazine();
                break;
            case "can_shoot":
                CanShoot();
                break;
        }
    }

    private void DetachMagazine()
    {
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }
    private void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);
    }
    private void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }
    private void AttachMagazine()
    {
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        weapon.ammoCount = weapon.clipSize;
        rigController.ResetTrigger("reload_weapon");
    }
    private void CanShoot()
    {
        weapon.canShoot = true;
    }
}
