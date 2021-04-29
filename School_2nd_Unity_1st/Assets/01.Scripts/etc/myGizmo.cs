using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myGizmo : MonoBehaviour
{
    public Color color = Color.red;
    public float radius = 0.3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
