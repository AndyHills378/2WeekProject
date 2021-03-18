using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public bool isFiring = false;
    private bool canShoot = true;
    //[SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDrop;
    [SerializeField] private float shotDelay;
    [SerializeField] private Transform shotMiss;
    public ParticleSystem muzzleFlash;
    public ParticleSystem wallHitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;


    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifeTime = 3f;

    private Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5*g*t^2
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition)+ (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
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

    public void StartFiring()
    {
        isFiring = true;
        if (canShoot)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        canShoot = false;
        muzzleFlash.Emit(1);
        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        StartCoroutine(ShootDelay());
    }

    public void UpdateBullet(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }
    void DestroyBullets()
    {
        bullets.RemoveAll(bullet=>bullet.time >= maxLifeTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = (end - start).magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            wallHitEffect.transform.position = hitInfo.point;
            wallHitEffect.transform.forward = hitInfo.normal;
            wallHitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;
        }
        else
        {
            bullet.tracer.transform.position = shotMiss.position;
            bullet.time = maxLifeTime;
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
}