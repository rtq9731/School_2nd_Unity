using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    // �Ŀ� / �Ÿ�
    [SerializeField]
    public float power;
    [SerializeField]
    public float range;
    [SerializeField]
    public Camera fpsCam;

    [SerializeField]
    [Header("��ƼŬ �ý���")]
    public ParticleSystem muzzleFlash;

    [SerializeField]
    [Header("�¾��� �� �߻��� ��ƼŬ")]
    public GameObject impactEffect;

    [SerializeField]
    [Header("�Ѿ��� ���������� �̴� ��")]
    public float impactFos = 60f;

    [SerializeField]
    [Header("�����ΰ�?")]
    public bool isAuto = false;

    [SerializeField] [Header("���� �� �߻� �ӵ�")]
    public float fireRate = 15f;
    private float nextTimeToFire;

    [SerializeField]
    [Header("���� ������ / ������")]
    public LineRenderer lineLaser = null;
    [SerializeField]
    [Header("ũ�ν����")]
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
            //���� �������� ���� ����
            lineLaser.SetPosition(1, lineLaser.transform.InverseTransformPoint(laserHit.point));
            //���� ������ ������ ������ ����ȭ.
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
                 Debug.Log("����");
             }
                 else if (Input.GetKeyDown(KeyCode.V) && isAuto)
             {
                 isAuto = false;
                 Debug.Log("�ܹ�");
             }
        } //�ڵ����� �˻�


        if(Input.GetButton("Fire1") && Time.time > nextTimeToFire && isAuto && !isReload)
        {
            //nextTimeToFire = Time.time + fireRate; // 1�� ���� �̻��� �����̸� �ش�.
            nextTimeToFire = Time.time + (1 / fireRate); //0.�� ������ �����̸� �ش�.
            Shoot();

            reloadMaxTime = nextTimeToFire - Time.time;
            reloadCurTime = 0;
            isReload = true;

        }

        if (Input.GetButtonDown("Fire1") && !isAuto && !isReload)
        {
            nextTimeToFire = Time.time + (1 / fireRate); //0.�� ������ �����̸� �ش�.
            Shoot();

            reloadMaxTime = nextTimeToFire - Time.time;
            reloadCurTime = 0;
            isReload = true;
        }

        // ������ ������ ó��
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

            //���̰� ���� ������ ���� ������Ʈ�� ��ũ��Ʈ�� �о�ͼ�, ������ ó���� ���ش�.
            TargetBox tb = hit.transform.GetComponent<TargetBox>();
            if(tb != null)
            {
                tb.TakeDamage(power);
            }

            // ���� �ڽ��� �δ�.

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactFos);
            }

            //�Ѿ��� ���� ������ ��ƼŬ �� �������.
            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObj, 2f);
        }
    }
}
