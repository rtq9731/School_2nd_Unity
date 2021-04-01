using UnityEngine;

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

    [SerializeField] [Header("���� �� �߻� �ӵ�")]
    public float fireRate = 3f;
    private float nextTimeToFire;

    [SerializeField]
    [Header("���� ������ / ������")]
    public LineRenderer lineLaser = null;
    [SerializeField]
    [Header("ũ�ν����")]
    public GameObject crosshair = null;

    private void Update()
    {
        crosshair.SetActive(true);
        RaycastHit laserHit;
        Ray laserRay = new Ray(lineLaser.transform.position, lineLaser.transform.forward);
        if(Physics.Raycast(laserRay, out laserHit))
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

        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
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
