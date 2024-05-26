using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerSO;

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

    private float weaponNumber;
    //health 
    public float curHealth;
    private PArmor curArmor;
    private float curArmorValue;


    // Start is called before the first frame update
    void Start()
    {
        curHealth = playerStats.pHealth;
        curArmor = playerStats.pArmor;
        switch (curArmor)
        {
            case PArmor.None: curArmorValue = 0; break; 
            
            case PArmor.Light: curArmorValue = 0.25f; break;

            case PArmor.Medium: curArmorValue = 0.5f; break;

            case PArmor.Heavy: curArmorValue = 0.75f; break;


        }
        AllWeapons = gameObject.GetComponentsInChildren<WeaponBase>();
        Primary = AllWeapons[0];
        Secondary = AllWeapons[1];
        curWeapon = Primary;
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        InputManager.Init(this);
        InputManager.EnableInGame();
    }
    
    
    private void FixedUpdate()
    {
        smoothedMoveDir = Vector2.SmoothDamp(smoothedMoveDir, moveDir, ref smoothedMoveVelo, 0.1f);
        rb.velocity = smoothedMoveDir * moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if(curHealth < 0)
        {
            this.gameObject.SetActive(false);
        }
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
        curWeapon.StartCoroutine(curWeapon.GetTryReload());
    }
    public void Shoot()
    {
        
        weaponShootToogle = !weaponShootToogle;
        if (weaponShootToogle) curWeapon.startShooting();
        else curWeapon.stopShooting();
    }

    public void switchWeapon(float w)
    {
        weaponNumber = w;
        Debug.Log(weaponNumber);
        switch (weaponNumber) 
        { 
            case 0:
                if (curWeapon == Primary)
                {
                    curWeapon = Secondary;
                    Secondary.UpdateAmmo();
                    return;
                }
                else if (curWeapon == Secondary)
                {
                    curWeapon = Primary;
                    Primary.UpdateAmmo();
                    return;
                }
                break;

            case 1:
                curWeapon = Primary;
                Primary.UpdateAmmo();
                break;

            case 2:
                curWeapon = Secondary;
                Secondary.UpdateAmmo();
                break;
        }

    
        }
     
      
    

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        
    }

    

}
