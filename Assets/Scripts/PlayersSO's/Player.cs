using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Other classes
    Rigidbody2D rb;
    [SerializeField] PlayerSO playerStats;
     WeaponBase Primary;
     WeaponBase Secondary;
    WeaponBase curWeapon;

    WeaponBase[] AllWeapons;

    private bool weaponShootToogle;

    private Camera cam;
    private Vector3 mousePos;

    //Movement
    [SerializeField] float moveSpeed;
    private Vector2 smoothedMoveDir;
    private Vector2 smoothedMoveVelo;
    private Vector2 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        AllWeapons = gameObject.GetComponentsInChildren<WeaponBase>();
        Primary = AllWeapons[0];
        
        curWeapon = Primary;
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        InputManager.Init(this);
        InputManager.EnableInGame();
    }
    private void OnDisable()
    {
        InputManager.DisableInGame();
    }
    private void FixedUpdate()
    {
        smoothedMoveDir = Vector2.SmoothDamp(smoothedMoveDir, moveDir, ref smoothedMoveVelo, 0.1f);
        rb.velocity = smoothedMoveDir * moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(InputManager.GetMousePos());
        Vector3 rotation = mousePos - transform.position;

        float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotz );
    }
    public void SetMoveDirection(Vector2 newDir)
    {
        moveDir = newDir;
    }
    public void Reload()
    {
        curWeapon.StartCoroutine(curWeapon.TryReload());
    }
    public void Shoot()
    {
        weaponShootToogle = !weaponShootToogle;
        if (weaponShootToogle) curWeapon.startShooting();
        else curWeapon.stopShooting();
    }
}
