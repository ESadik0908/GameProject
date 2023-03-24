using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStats
{
    float health { get; set; }
    
    void Damage(float damage);

    void Die();

}
