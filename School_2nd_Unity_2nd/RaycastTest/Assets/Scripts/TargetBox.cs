using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBox : MonoBehaviour
{
    [SerializeField]
    public float hp = 50f;

    public GameObject destroyObj = null;

    public void TakeDamage(float amount)
    {
        Debug.Log("데미지 받음!");
        hp -= amount;

        if(hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(destroyObj, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

}
