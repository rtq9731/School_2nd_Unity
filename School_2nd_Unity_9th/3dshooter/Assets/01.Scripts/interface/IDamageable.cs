using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    //인터페이스의 매서드는 다 public
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal);
}
