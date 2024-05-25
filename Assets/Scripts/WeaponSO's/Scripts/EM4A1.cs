using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM4A1 : WeaponBase.EnemyWeapons
{
    [SerializeField] EProjectile normalBullet;
    [SerializeField] Transform firingPostition;
    protected override void Attack(float percent)
    {
        EProjectile rb = Instantiate(normalBullet, firingPostition.position,Quaternion.identity);
        rb.Init(percent);
        Destroy(rb.gameObject, 4);
    }
}
