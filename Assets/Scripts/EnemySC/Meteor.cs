/*
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class Meteor : Enemy
{

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private ScriptableObjExample powerUpSpawner;
     
    
    private float speed;

    void Start()
    {
        speed=Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity=Vector2.down*speed;
    }

    void Update()
    {
        transform.Rotate(0,0,rotateSpeed*Time.deltaTime);
    }
    public override void HurtSequence()
    {
        //base.HurtSequence();
    }

    public override void DeathSequence()
    {
        base.DeathSequence();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        if(powerUpSpawner!=null)
        {
            powerUpSpawner.SpawnPowerUp(transform.position);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D otherColl)
    {
        if(otherColl.gameObject.CompareTag("Player"))
        {
            //Destroy(otherColl.gameObject);
            PlayerStats player=otherColl.gameObject.GetComponent<PlayerStats>();
            player.PlayerTakeDamage(damage);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
*/

using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.Pool; // 1. Havuz kütüphanesini ekledik!

public class Meteor : Enemy
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private ScriptableObjExample powerUpSpawner;
     
    private float speed;

    // 2. Havuz referansımızı (kimlik kartı) tanımladık
    private ObjectPool<Meteor> meteorPool;

    // Spawner'ın meteora patronunun kim olduğunu söylediği fonksiyon
    public void SetPool(ObjectPool<Meteor> pool)
    {
        meteorPool = pool;
    }

    // 3. KRİTİK DEĞİŞİKLİK: Start yerine OnEnable kullandık!
    protected override void OnEnable()
    {
        // Meteor her havuzdan çağrıldığında yepyeni ve rastgele bir hıza sahip olacak.
        base.OnEnable(); // Ana sınıftaki can yenileme (health reset) çalışsın
        speed = Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity = Vector2.down * speed;
    }

    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    public override void HurtSequence()
    {
        //base.HurtSequence();
    }

    public override void DeathSequence()
    {
        base.DeathSequence();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        if(powerUpSpawner != null)
        {
            powerUpSpawner.SpawnPowerUp(transform.position);
        }
        
        // 4. Destroy YERİNE Havuza İade:
        if(gameObject.activeSelf)
        {
            meteorPool.Release(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherColl)
    {
        if(otherColl.gameObject.CompareTag("Player"))
        {
            PlayerStats player = otherColl.gameObject.GetComponent<PlayerStats>();
            player.PlayerTakeDamage(damage);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            
            // 4. Destroy YERİNE Havuza İade:
            if(gameObject.activeSelf)
            {
                meteorPool.Release(this);
            }
        }
    }

    private void OnBecameInvisible()
    {
        // 4. Destroy YERİNE Havuza İade:
        if(gameObject.activeSelf)
        {
            meteorPool.Release(this);
        }
    }
}