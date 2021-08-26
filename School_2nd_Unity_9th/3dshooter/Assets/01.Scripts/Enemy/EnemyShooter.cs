using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private Gun gun;
    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("fire");
    private readonly int hashReload = Animator.StringToHash("reload");

    public bool isFire = false;
    public bool isReload = false;
    private float nextFire = 0.0f;
    private WaitForSeconds wsReload;

    private Transform playerTr;
    private readonly float damping = 10.0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerTr = GameManager.Instance.GetPlayer();
        gun = GetComponentInChildren<Gun>();
        wsReload = new WaitForSeconds(gun.reloadTime);
    }

    private void Update()
    {
        if(isFire && !isReload)
        {
            Quaternion rot = Quaternion.LookRotation(playerTr.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);

            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + gun.timeBetFire + Random.Range(0f, gun.timeBetFire - 0.1f); // 시간을 두고 발사하도록 함
            }
        }
    }

    private void Fire()
    {
        anim.SetTrigger(hashFire);
        gun.Fire();

        if(gun.magAmmo <= 0 && !isReload)
        {
            gun.Reload();
            isReload = true;
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        anim.SetTrigger(hashReload);
        yield return wsReload;
        isReload = false;
    }

}
