using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInEnemyAI : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float gravity;
    [SerializeField] GameObject projectTile;
    [SerializeField] Transform gun;

    Vector3 moveDir = Vector3.zero;

    CharacterController charCon;

    private void Awake()
    {
        charCon = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
    }

    private void Update()
    {
        //이동코드
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.z = Input.GetAxisRaw("Vertical") * moveSpeed;

        if (charCon.isGrounded && Input.GetButton("Jump"))
        {
            moveDir.y = jumpPower;
        }
        moveDir.y -= gravity * Time.deltaTime;

        charCon.Move(moveDir * Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Shot();
            }
        }

    }

    private void Shot()
    {
        Rigidbody rb = Instantiate(projectTile, gun.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);

        Destroy(rb.gameObject, 5f);
    }
}
