using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    // 파워 / 거리
    [SerializeField]
    public float power;
    [SerializeField]
    public float range;
    [SerializeField]
    public Camera fpsCam;

    [SerializeField]
    [Header("파티클 시스템")]
    public ParticleSystem muzzleFlash;

    [SerializeField]
    [Header("맞았을 시 발생할 파티클")]
    public GameObject impactEffect;

    [SerializeField]
    [Header("총알이 물리적으로 미는 힘")]
    public float impactFos = 60f;

    [SerializeField]
    [Header("연사인가?")]
    public bool isAuto = false;

    [SerializeField] [Header("연사 시 발사 속도")]
    public float fireRate = 15f;
    private float nextTimeToFire;

    [SerializeField]
    [Header("라인 렌더러 / 레이저")]
    public LineRenderer lineLaser = null;
    [SerializeField]
    [Header("크로스헤어")]
    public GameObject crosshair = null;

    [SerializeField]
    [Header("")]
    public Image reloadImage = null;
    [SerializeField]
    [Header("")]
    public bool isReload = false;

    private float reloadCurTime = 0f;
    private float reloadMaxTime = 0f;

    private void Update()
    {
        crosshair.SetActive(true);
        RaycastHit laserHit;
        Ray laserRay = new Ray(lineLaser.transform.position, lineLaser.transform.forward);
        if (Physics.Raycast(laserRay, out laserHit))
        {
            //라인 렌더러의 길이 조정
            lineLaser.SetPosition(1, lineLaser.transform.InverseTransformPoint(laserHit.point));
            //라인 렌더러 끝점에 조준점 동기화.
            Vector3 crosshairLocation = Camera.main.WorldToScreenPoint(laserHit.point);
            crosshair.transform.position = crosshairLocation;
        }
        else
        {
            crosshair.SetActive(false);
        }

        

        { 
             if (Input.GetKeyDown(KeyCode.V) && !isAuto)
             {
                 isAuto = true;
                 Debug.Log("연사");
             }
                 else if (Input.GetKeyDown(KeyCode.V) && isAuto)
             {
                 isAuto = false;
                 Debug.Log("단발");
             }
        } //자동인지 검사


        if(Input.GetButton("Fire1") && Time.time > nextTimeToFire && isAuto && !isReload)
        {
            //nextTimeToFire = Time.time + fireRate; // 1초 단위 이상의 딜레이를 준다.
            nextTimeToFire = Time.time + (1 / fireRate); //0.초 단위의 딜레이를 준다.
            Shoot();

            reloadMaxTime = nextTimeToFire - Time.time;
            reloadCurTime = 0;
            isReload = true;

        }

        if (Input.GetButtonDown("Fire1") && !isAuto && !isReload)
        {
            nextTimeToFire = Time.time + (1 / fireRate); //0.초 단위의 딜레이를 준다.
            Shoot();

            reloadMaxTime = nextTimeToFire - Time.time;
            reloadCurTime = 0;
            isReload = true;
        }

        // 재장전 게이지 처리
        if(isReload)
        {
            reloadCurTime += Time.deltaTime;
            reloadImage.fillAmount = reloadCurTime / reloadMaxTime;
            if(reloadCurTime / reloadMaxTime > 1)
            {
                reloadImage.fillAmount = 0;
                isReload = false;
            }
        }

    }

    private void Shoot()
    {

        muzzleFlash.Play();

        RaycastHit hit;
        if(Physics.Raycast(lineLaser.transform.position, lineLaser.transform.forward, out hit, range) )
        {
            Debug.Log(hit.transform.name);

            //레이가 맞은 지점의 게임 오브젝트의 스크립트를 읽어와서, 데미지 처리를 해준다.
            TargetBox tb = hit.transform.GetComponent<TargetBox>();
            if(tb != null)
            {
                tb.TakeDamage(power);
            }

            // 맞은 박스를 민다.

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactFos);
            }

            //총알이 맞은 지점에 파티클 후 사라지게.
            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObj, 2f);
        }
    }
}
