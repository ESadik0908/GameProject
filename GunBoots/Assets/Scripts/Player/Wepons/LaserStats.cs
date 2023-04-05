using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStats : MonoBehaviour
{
    [SerializeField] private float initDamage;
    [SerializeField] private float initAmmo;

    private void Start()
    {
        damage = initDamage;
        ammo = initAmmo;
    }

    public float damage { get; private set; }
    public float ammo { get; private set; }
}
