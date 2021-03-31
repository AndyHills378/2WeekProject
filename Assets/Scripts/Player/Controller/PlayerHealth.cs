using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [SyncVar]
    [HideInInspector] public float health = 100f;

    [SerializeField] private Image damageImage;
    private float imageLifeSpan = 2f;

    public void Update()
    {
        
    }

    [Client]
    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(flashDamageImage());
        damageImage.gameObject.SetActive(false);
    }

    [Client]
    public IEnumerator flashDamageImage()
    {
        damageImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(imageLifeSpan);
    }
}
