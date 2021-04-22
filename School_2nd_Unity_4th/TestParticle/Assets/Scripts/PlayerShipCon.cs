using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipCon : MonoBehaviour
{
    [SerializeField] float controlSpeed = 25f;
    [SerializeField] float xLimitRange = 7f;
    [SerializeField] float yLimitRange = 7f;

    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float positionYawFactor = 2f;
    [SerializeField] float controlRollFactor = -20f;

    [SerializeField] GameObject[] laserobjs;

    float xAxisVal, yAxisVal;

    void Update()
    {
        // 이동 관련 코드
        xAxisVal = Input.GetAxis("Horizontal");
        yAxisVal = Input.GetAxis("Vertical");

        float xOffset = xAxisVal * Time.deltaTime * controlSpeed;
        float xPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(xPos, -xLimitRange, xLimitRange);

        float yOffset = yAxisVal * Time.deltaTime * controlSpeed;
        float yPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(yPos, -yLimitRange, yLimitRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);

        // 회전 관련 코드
        float pitchDueToPos = transform.localPosition.y * positionPitchFactor;
        float pitchDueToCotrolAxis = yAxisVal * controlPitchFactor;

        float pitch = pitchDueToPos + pitchDueToCotrolAxis;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xAxisVal * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

        ProcessFire();

    }

    private void ProcessFire()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLaserActive(true);
        }
        else
        {
            SetLaserActive(false);
        }
    }

    private void SetLaserActive(bool isActive)
    {
        foreach(GameObject laser in laserobjs)
        {
            // laser.SetActive(isActive);
            var emissonValue = laser.GetComponent<ParticleSystem>().emission;
            emissonValue.enabled = isActive;
        }
    }

}
