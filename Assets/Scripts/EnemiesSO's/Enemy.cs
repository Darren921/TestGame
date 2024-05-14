using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySO enemyStats;
    [SerializeField] WeaponSO weaponStats;
    WeaponBase curWeapon;
    [SerializeField] Transform player;
    [SerializeField] float speed = 400f;
    private float sightRange, attackRange;
    private float nextWaypointDistance = 1f;

    Pathfinding.Path path;
    int currentWaypoint = 0;
    bool reachedWaypoint;

    Rigidbody2D rb;
    Seeker seeker;


    

    // Start is called before the first frame update


    private void Awake()
    {
        sightRange = enemyStats.sightRange;
        attackRange = weaponStats.attackRange;
        
        curWeapon = GetComponentInChildren<WeaponBase>();
        player = GameObject.FindObjectOfType<Player>().transform;
         rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

                    
    }

    void  UpdatePath()
    {
        if(seeker.IsDone())
        seeker.StartPath(rb.position, player.position, OnPathComplete);

    }


    private void OnPathComplete(Pathfinding.Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.right = player.position - transform.position;
        if (path == null) return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedWaypoint = true;
            return;
        }
        else
        {
            reachedWaypoint= false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    
   
}
