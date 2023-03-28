using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStats : MonoBehaviour
{
    [SerializeField] private float initDamage;
    [SerializeField] private float initAmmo;

    private void Start()
    {
        damage = initDamage;
    }

    public float damage { get; private set; }
    public int ammo { get; private set; }
}
