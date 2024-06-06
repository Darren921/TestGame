using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EProjectile : MonoBehaviour
{
    [SerializeField] private float shootForce;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 FireDirection;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType <Player>().transform;
        Vector3 direction = target.position - transform.position;
        Vector3 rotation = transform.position - target.position;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
        FireDirection = new Vector2(direction.x, direction.y).normalized;

    }
    public void Init(float chargePercent)
    {
        rb.AddForce(shootForce * chargePercent * FireDirection, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            collision.GetComponent<Player>().curHealth -= 5;
            Destroy(rb.gameObject);
        }
    }
}
