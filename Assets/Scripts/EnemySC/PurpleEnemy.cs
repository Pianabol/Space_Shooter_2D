/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleEnemy : Enemy
{

    [SerializeField] private float speed;
    [SerializeField] private float shootInterval;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform leftCanon;
    [SerializeField] private Transform rightCanon;

    private float shootTimer=0;

    void Start()
    {
        rb.linearVelocity=Vector2.down*speed;

    }

    void Update()
    {
        shootTimer+=Time.deltaTime;
        if(shootTimer>=shootInterval)
        {
            Instantiate(bulletPrefab,leftCanon.position, Quaternion.identity);
            Instantiate(bulletPrefab,rightCanon.position, Quaternion.identity);
            shootTimer=0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().PlayerTakeDamage(damage);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public override void HurtSequence()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("DMG"))
        {   
            anim.SetTrigger("Damage");
            return;
           
        }  
        
    }

    public override void DeathSequence()
    {
        base.DeathSequence();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void onBecameInvisible()
    {
        Destroy(gameObject);
    }   


}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleEnemy : Enemy
{
    [SerializeField] private float speed;
    [SerializeField] private float shootInterval;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform leftCanon;
    [SerializeField] private Transform rightCanon;

    private float shootTimer = 0;

    // Start YERİNE OnEnable kullanıyoruz!
    protected override void OnEnable()
    {
        base.OnEnable(); // Ana sınıftaki can yenileme (health reset) çalışsın
        rb.linearVelocity = Vector2.down * speed; // Yeni hız atansın
        shootTimer = 0; // Havuzdan her çıktığında zamanlayıcıyı sıfırla ki anında ateş etmesin
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        if(shootTimer >= shootInterval)
        {
            Instantiate(bulletPrefab, leftCanon.position, Quaternion.identity);
            Instantiate(bulletPrefab, rightCanon.position, Quaternion.identity);
            shootTimer = 0;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().PlayerTakeDamage(damage);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            
            // Destroy YERİNE
            ReturnToPool();
        }
    }

    public override void HurtSequence()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("DMG"))
        {   
            anim.SetTrigger("Damage");
            return;
        }  
    }

    public override void DeathSequence()
    {
        base.DeathSequence();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        
        // Destroy YERİNE
        ReturnToPool();
    }

    // DİKKAT: Metodun adını burada da düzelttim! (Büyük O)
    private void OnBecameInvisible()
    {
        // Destroy YERİNE
        ReturnToPool();
    }   
}