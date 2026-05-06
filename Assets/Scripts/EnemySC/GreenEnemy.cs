using UnityEngine;

public class GreenEnemy : Enemy
{
    [SerializeField] private float speed;

    // Start YERİNE OnEnable kullanıyoruz! Ana sınıftaki OnEnable'ı ezmemesi için base.OnEnable() diyoruz.
    protected override void OnEnable()
    {
        base.OnEnable(); // Ana sınıftaki can yenileme (health reset) çalışsın
        rb.linearVelocity = Vector2.down * speed; // Yeni hız atansın
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
        base.DeathSequence(); // Skoru artırır
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        
        // Destroy YERİNE
        ReturnToPool();
    }

    // DİKKAT: Metodun adını düzelttim! Küçük 'o' harfiyle yazmıştın, Unity bunu tanımazdı.
    private void OnBecameInvisible()
    {
        // Destroy YERİNE
        ReturnToPool();
    }   
}