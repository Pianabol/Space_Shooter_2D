using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniBullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float damage;

    [SerializeField] private Rigidbody2D rb;
     void Start()
    {
         rb.linearVelocity = transform.up * speed;
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().PlayerTakeDamage(damage);
            Destroy(gameObject);
        }
    }


    private void onBecameInvisible()
    {
        Destroy(gameObject);
    }

    
}
