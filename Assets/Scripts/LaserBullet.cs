using UnityEngine;
using System.Collections;
using System.Collections.Generic;   
using UnityEngine.Pool;

public class LaserBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private Rigidbody2D rb;

    private ObjectPool<LaserBullet> referencePool;

    public void SetPool(ObjectPool<LaserBullet> pool)
    {
        referencePool=pool;
    }
    void OnEnable()
    {
        rb.linearVelocity= transform.up*speed;
    }

    private void OnDisable() 
    {
        transform.rotation=Quaternion.Euler(0,0,0);
    }
    
    private void OnTriggerEnter2D(Collider2D collisions)
    {
        Enemy enemy=collisions.GetComponent<Enemy>();
        enemy.TakeDamage(damage);
        if(gameObject.activeSelf)
        {
            referencePool.Release(this);
        }
        //Destroy(gameObject);
    }
    
    public void SetDirectionAndSpeed()
    {
        rb.linearVelocity= transform.up*speed;
    }
    private void OnBecameInvisible() 
    {
        //Destroy(gameObject);
        if(gameObject.activeSelf)
        {
            referencePool.Release(this);
        }
    }
    void Update()
    {
        
    }
}
