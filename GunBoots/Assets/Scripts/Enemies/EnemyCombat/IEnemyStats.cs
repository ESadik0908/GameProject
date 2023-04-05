using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStats
{
    Vector3 Position { get; }

    float contactDamage { get; }

    void Damage(float damage);

    void Die();
}
