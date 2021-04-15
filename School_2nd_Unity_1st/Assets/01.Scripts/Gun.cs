using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Ready,
    Empty,
    Reloading
}

public class Gun : MonoBehaviour
{
    public State state { get; private set; }
     
    public Transform firePos;
    public ParticleSystem muzzleFlash;
    public ParticleSystem sheellEjectEffect;
    public float bulletLineEffectTiem = 0.03f;

    public LineRenderer bulletLineRenderer;
    public float damage = 25;
    public float fireDistance = 50f;
    public int magCapacity = 10;
    public int magAmmo;
    public float timeBetFire = 0.12f;
    public float reloadTime = 1.0f;
    public float lastFireTime;

    private void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    public void Shot()
    {

        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, fireDistance))
        {
            IDamageable target = hit.transform.GetComponent<IDamageable>();
            if(target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPos = hit.point;
        }else
        {
            hitPos = firePos.position + firePos.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPos));

        magAmmo--;

        if (magAmmo <= 0)
            state = State.Empty;
    }

    public IEnumerator ShotEffect(Vector3 hitPos)
    {
        muzzleFlash.Play();
        sheellEjectEffect.Play();
        bulletLineRenderer.SetPosition(1, bulletLineRenderer.transform.InverseTransformPoint(hitPos));
        bulletLineRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(bulletLineEffectTiem);
        bulletLineRenderer.gameObject.SetActive(false);
    }

    public bool Reload()
    {

        if(State.Reloading == state || magAmmo >= magCapacity)
            return false;

        StartCoroutine(ReloadRoutine());
            
        return true;
    }

    public IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        state = State.Ready;
    }

}
