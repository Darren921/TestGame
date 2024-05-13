using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject);
        print(collision.gameObject.tag);
        if (collision.gameObject.transform.CompareTag("Wall"))
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
