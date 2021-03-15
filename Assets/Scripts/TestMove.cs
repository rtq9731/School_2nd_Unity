using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rigid.velocity = transform.forward.normalized * z * speed;
        rigid.rotation = rigid.rotation * Quaternion.Euler(0, x, 0);
    }
}
