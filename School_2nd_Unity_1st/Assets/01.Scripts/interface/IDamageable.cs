using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // 인터페이스의 메소드는 다 public
    void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal);
}
