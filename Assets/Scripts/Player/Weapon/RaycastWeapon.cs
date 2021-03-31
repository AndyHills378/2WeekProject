using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaycastWeapon : NetworkBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    [Header("References")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDrop;
    [SerializeField] private float shotDelay;
    [SerializeField] private Transform shotMiss;
    [SerializeField] private AudioClip gunShot;
    [SerializeField] private GameObject player;
    private WeaponRecoil WeaponRecoil;
    private AudioSource audioSource;
    public ParticleSystem muzzleFlash;
    public ParticleSystem wallHitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public GameObject magazine;
    public TextMeshProUGUI ammoInClipText;

    [Header("Settings")]
    [SerializeField] private float maxRange; // Gun Shooting range
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    float maxLifeTime = 4f;
    public int ammoCount;
    public int clipSize;
    [HideInInspector] public bool isFiring = false;
    [HideInInspector] public bool canShoot = true;



    Ray ray;
    RaycastHit hitInfo;
    List<Bullet> bullets = new List<Bullet>();

    [ClientCallback]
    private void Awake()
    {
        WeaponRecoil = GetComponent<WeaponRecoil>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    [Client]
    private Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5*g*t^2
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    // Update is called once per frame
    public void Update()
    {
        UpdateBullet(Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            StartFiring();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopFiring();
        }

        if (ammoCount == 0)
        {
            ammoInClipText.text = "30";
        }
        else
        {
            ammoInClipText.text = ammoCount.ToString();
        }
    }

    [Client]
    public void StartFiring()
    {
        isFiring = true;
        Shoot();
    }

    [Client]
    public void StopFiring()
    {
        WeaponRecoil.StopRecoil();
        isFiring = false;
    }

    [Command]
    private void CmdShoot()
    {
        RpcDoShootEffects();
    }

    [ClientRpc]
    public void RpcDoShootEffects()
    {
        muzzleFlash.Emit(1);
        audioSource.PlayOneShot(gunShot, .5f);
    }

    [Client]
    private void Shoot()
    {
        if (ammoCount <= 0 || !canShoot)
        {
            return;
        }
        CmdShoot();
        ammoCount--;

        canShoot = false;
        WeaponRecoil.GenerateRecoil();
        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        StartCoroutine(ShootDelay());

    }

    [Client]
    private void UpdateBullet(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    [Client]
    private void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    [Client]
    void DestroyBullets()
    {
        bullets.RemoveAll(bullet=>bullet.time >= maxLifeTime);
    }

    [Client]
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, maxRange))
        {
            wallHitEffect.transform.position = hitInfo.point;
            wallHitEffect.transform.forward = hitInfo.normal;
            wallHitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;

            if ((hitInfo.collider.gameObject.tag == "Blue Team" || hitInfo.collider.gameObject.tag == "Red Team") && hitInfo.collider.gameObject.tag != this.gameObject.tag)
            {
                int damage = Random.Range(minDamage, maxDamage);
                int hitNetworkID = hitInfo.collider.gameObject.GetComponent<PlayerManager>().networkId;
                if (hitInfo.collider.gameObject.GetComponent<PlayerManager>().AdjustHealth(damage))
                {
                    GetComponent<PlayerManager>().AdjustKills();
                    hitInfo.collider.gameObject.GetComponent<PlayerManager>().AdjustDeaths();
                }
            }
        }
        else
        {
            bullet.tracer.transform.position = shotMiss.position;
            bullet.time = maxLifeTime;
        }
    }

    [Client]
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
}
