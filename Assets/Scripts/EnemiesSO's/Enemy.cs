using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour 
{
    
    [SerializeField] EnemySO enemyStats;
    [SerializeField] WeaponSO weaponStats;
    WeaponBase.EnemyWeapons curWeapon;
    [SerializeField] Transform player;
    [SerializeField] float speed = 400f;
    private float sightRange, attackRange;
    private float nextWaypointDistance = 0.5f;

    Pathfinding.Path path;
    int currentWaypoint = 0;
    bool reachedWaypoint;
    bool playerInRange;
    bool playerInSightRange;
    private RaycastHit2D playerInLOS;
    Rigidbody2D rb;
    Seeker seeker;
    [SerializeField] LayerMask whatIsPlayer, whatIsWall;
    float patrolRange;

    bool onCoolDown;
    private bool isAttacking;
    private bool isChasing;
    internal float curHealth;


    // Start is called before the first frame update


    private void Awake()
    {
        sightRange = enemyStats.sightRange;
        attackRange = weaponStats.attackRange;
        curHealth = enemyStats.eHealth;
        patrolRange = 3;
         curWeapon = gameObject.GetComponentInChildren<WeaponBase.EnemyWeapons>();
        player = GameObject.FindObjectOfType<Player>().transform;
         rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        Patrolling();
                    
    }

  

    private void OnPathComplete(Pathfinding.Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        if (p.error) return;
    }

  
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.right = player.position - transform.position;
        if (path == null) return;

        if(currentWaypoint >= path.vectorPath.Count )
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
        private void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
        playerInLOS = Physics2D.Linecast(transform.position, player.position, whatIsWall);
       
        Debug.DrawLine(transform.position, player.position,Color.green);

        if (!playerInRange && !playerInSightRange && !onCoolDown) StartCoroutine(Patrolling());

        if (!playerInLOS.transform.CompareTag("Wall"))
        {
            if (playerInSightRange && !playerInRange) Chasing();
            if (playerInRange && playerInSightRange) StartCoroutine(Attacking());
        }
       
        if (curWeapon.ammoLeft == 0 || curWeapon.isJammed)
        {
            StartCoroutine(curWeapon.GetTryReload());
            isAttacking = false;
        }

        if (curHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator Attacking()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            currentWaypoint++;

            if (seeker.IsDone())
                seeker.StartPath(rb.position, rb.position, OnPathComplete);
            curWeapon.startShooting();
           
        }
        yield return new WaitUntil(() => !playerInRange);
        isAttacking = false;
    }


    private void Chasing()
    {
        if(isAttacking == false && curWeapon.currentFireTimer != null)
        curWeapon.stopShooting();
        
       
        currentWaypoint++;

        seeker.CancelCurrentPathRequest();
            if (seeker.IsDone())
                seeker.StartPath(rb.position, player.position, OnPathComplete);
       
        
       

    }
    private Vector2 SetPoint()
    {
        float randX = UnityEngine.Random.Range(-patrolRange, patrolRange);
        float randY = UnityEngine.Random.Range(-patrolRange, patrolRange);
        return new Vector2(randX, randY);
    }
         
    private IEnumerator Patrolling()
    {
        if (!onCoolDown)
        {
            onCoolDown = true;
            SetPoint();
            if (seeker.IsDone())
                seeker.StartPath(rb.position, rb.position + new Vector2(SetPoint().x, SetPoint().y), OnPathComplete);
        }
        yield return new WaitForSeconds(5);
        onCoolDown = false;
    }


}
