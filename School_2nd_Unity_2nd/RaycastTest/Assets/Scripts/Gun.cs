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

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range) )
        {
            Debug.Log(hit.transform.name);

            //���̰� ���� ������ ���� ������Ʈ�� ��ũ��Ʈ�� �о�ͼ�, ������ ó���� ���ش�.
            TargetBox tb = hit.transform.GetComponent<TargetBox>();
            if(tb != null)
            {
                tb.TakeDamage(power);
            }
        }
    }
}
