using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("레이저에 맞았다!");
    }

}
