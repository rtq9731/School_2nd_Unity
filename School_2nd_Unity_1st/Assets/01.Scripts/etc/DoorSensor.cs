using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    public float openTime = 2f;
    public float openSpeed = 5f;
    public Vector3 openDistance = new Vector3(4, 0, 0); // X축 기준으로 움직임

    private bool isOpen = false; // 문이 열렸는지 확인하는 변수
    private Vector3 originPoint; // 원본 위치
    private Vector3 targetPoint; // 이동할 위치

    void Start()
    {
        originPoint = transform.parent.position;
        targetPoint = transform.parent.position;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (isOpen) StopCoroutine("StayOpen");
            OpenDoor();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
            StartCoroutine("StayOpen");
    }

    private void OpenDoor()
    {
        if (isOpen) return;
        targetPoint = originPoint + openDistance;
        isOpen = true;
    }
    private void closeDoor()
    {
        if (!isOpen) return;
        targetPoint = originPoint;
        isOpen = false;
    }
    IEnumerator StayOpen()
    {
        yield return new WaitForSeconds(openTime);
        closeDoor();
    }

    void Update()
    {
        transform.position = originPoint;
        if (isOpen)
        {
            Vector3 nextPos = Vector3.Lerp(transform.parent.position, targetPoint, Time.deltaTime * openSpeed);
            transform.parent.position = nextPos;
        }
        else
        {
            if((transform.parent.position - originPoint).sqrMagnitude <= 0.01f)
            {
                return;
            }

            Vector3 nextPos = Vector3.Lerp(transform.parent.position, targetPoint, Time.deltaTime * openSpeed);
            transform.parent.position = nextPos;
        }
    }
}
