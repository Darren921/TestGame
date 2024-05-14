using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySO enemyStats;
    [SerializeField] WeaponSO weaponStats;
    WeaponBase curWeapon;
     NavMeshAgent agent;
    bool walkPointSet;
    Vector2 walkPoint;
    [SerializeField] Transform player;

    private float sightRange, attackRange;
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;
    private bool playerInSight, playerInAttackRange;
    private float walkPointRange;

    // Start is called before the first frame update


    private void Awake()
    {
        sightRange = enemyStats.sightRange;
        attackRange = weaponStats.attackRange;
        
        curWeapon = GetComponentInChildren<WeaponBase>();
        player = GameObject.FindObjectOfType<Player>().transform;
         
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    
   
}
