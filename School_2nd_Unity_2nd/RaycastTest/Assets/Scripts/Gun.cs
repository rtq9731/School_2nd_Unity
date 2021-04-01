using UnityEngine;

public class Gun : MonoBehaviour
{
    // 파워 / 거리
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

            //레이가 맞은 지점의 게임 오브젝트의 스크립트를 읽어와서, 데미지 처리를 해준다.
            TargetBox tb = hit.transform.GetComponent<TargetBox>();
            if(tb != null)
            {
                tb.TakeDamage(power);
            }
        }
    }
}
