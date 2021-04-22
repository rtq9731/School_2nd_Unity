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

    List<GameObject> soundObjs = new List<GameObject>();

    public State state { get; private set; }
     
    public Transform firePos;
    public ParticleSystem muzzleFlash;
    public ParticleSystem sheellEjectEffect;
    public float bulletLineEffectTiem = 0.03f;

    public LineRenderer bulletLineRenderer;
    public float damage = 25f;
    public float fireDistance = 50f;
    public int magCapacity = 10;
    public int magAmmo;
    public int numberOfObj = 0;
    public float timeBetFire = 0.12f;
    public float reloadTime = 1.0f;
    public float lastFireTime;

    [Header("Audio Clips")]
    public AudioClip reloadSound;
    public AudioClip fireSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public IEnumerator CallSoundObj(AudioClip clip)
    {
        if(soundObjs.Count < 10)
        {
            soundObjs.Add(Instantiate(Resources.Load("SoundObj") as GameObject));
        }

        GameObject nowSoundObj = soundObjs[numberOfObj].gameObject;
        nowSoundObj.SetActive(true);
        nowSoundObj.transform.position = this.transform.position;
        nowSoundObj.transform.parent = this.transform;
        nowSoundObj.GetComponent<AudioSource>().clip = clip;
        nowSoundObj.GetComponent<AudioSource>().Play();
        numberOfObj++;

        if (numberOfObj == 10)
            numberOfObj = 0;

        yield return new WaitForSeconds(1);
        nowSoundObj.SetActive(false);
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
        StartCoroutine(CallSoundObj(fireSound));

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
        StartCoroutine(CallSoundObj(reloadSound));
        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        state = State.Ready;
    }

}
