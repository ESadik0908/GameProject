using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStats
{
    Vector3 Position { get; }

    int contactDamage { get; }

    void Damage(float damage);

    void Die();
}
