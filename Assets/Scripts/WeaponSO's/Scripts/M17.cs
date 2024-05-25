using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M17 : WeaponBase
{
    [SerializeField] Projectile normalBullet;
    [SerializeField] Transform firingPostition;
    protected override void Attack(float percent)
    {
        Projectile rb = Instantiate(normalBullet, firingPostition.position, Quaternion.identity);
        rb.Init(percent);
        Destroy(rb.gameObject, 3);
    }
}
