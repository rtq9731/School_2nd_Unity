using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePostionSort : MonoBehaviour
{
    [SerializeField] private bool bRunOnce = true;

    [SerializeField] private float posOffsetY;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        posOffsetY = -transform.localPosition.y;
    }

    private void LateUpdate()
    {
        float precisionMultiplier = 5f;
        sr.sortingOrder = (int)(-(transform.position.y + posOffsetY) * precisionMultiplier);

        if(bRunOnce)
        {
            Destroy(this);
        }
    }
}
