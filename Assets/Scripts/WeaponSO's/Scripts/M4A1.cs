using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4A1 : WeaponBase
{
    [SerializeField] Projectile normalBullet;
    [SerializeField] Transform firingPostition;
    protected override void Attack(float percent)
    {
        
        var cam = Camera.main;
        var mousePos = cam.ScreenToWorldPoint(InputManager.GetMousePos());
        Projectile rb = Instantiate(normalBullet,firingPostition.position,Quaternion.identity);
        rb.Init(percent);
        Destroy(rb.gameObject,100);
    }
}
