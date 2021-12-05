using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] float moveSpeed = 30f;
    [SerializeField] float scrollAmount = 100f;
    [SerializeField] float minScroll = 2.5f;
    [SerializeField] float maxScroll = 20f;
    [SerializeField] Cinemachine.CinemachineVirtualCamera vCam = null;

    float orthographicSize = 0f;

    private void Start()
    {
        orthographicSize = vCam.m_Lens.OrthographicSize;
    }

    void Update()
    {
        HandleMovement();

        HandleZoom();
    }

    private void HandleZoom()
    {
        orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * scrollAmount;
        orthographicSize = Mathf.Clamp(orthographicSize, minScroll, maxScroll);
        // �ε巯�� ������ �ֱ� ���� �ڵ�
        float zoomSpeed = 5f;
        vCam.m_Lens.OrthographicSize = Mathf.Lerp(vCam.m_Lens.OrthographicSize, orthographicSize, Time.deltaTime * zoomSpeed);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // ī�޶� �̵� �ڵ�
        Vector3 moveDir = new Vector3(x, y).normalized;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
