using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public float range = 0.5f;
    private float damage;
    private DashStats dashStats;

    List<IEnemyStats> m_AllDamageables = new List<IEnemyStats>();
    private void OnEnable()
    {
        dashStats = GetComponent<DashStats>();
        damage = dashStats.damage;

        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < allScripts.Length; i++)
        {
            if (allScripts[i] is IEnemyStats)
                m_AllDamageables.Add(allScripts[i] as IEnemyStats);
        }
    }
    
    public void Explode()
    {
        for (int i = 0; i < m_AllDamageables.Count; i++)
        {
            if (Vector3.Distance(m_AllDamageables[i].Position, transform.position) < range)
                m_AllDamageables[i].Damage(damage);
        }
    }

}