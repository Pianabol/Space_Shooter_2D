/*
using UnityEngine;

public class PurpleBullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Rigidbody2D rb;
     void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        rb.linearVelocity=Vector2.down*speed;
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
*/

using UnityEngine;
using UnityEngine.Pool; // 1. Havuz kütüphanesi eklendi

public class PurpleBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Rigidbody2D rb;
    
    // 2. Havuz kimlik kartı tanımlandı
    private ObjectPool<PurpleBullet> referencePool;

    // Merkezi yöneticinin mermiye patronunu atadığı fonksiyon
    public void SetPool(ObjectPool<PurpleBullet> pool)
    {
        referencePool = pool;
    }

    // 3. GetComponent işlemini Awake'e aldık (Sadece yaratılırken 1 kez çalışır)
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 4. Start YERİNE OnEnable kullandık (Havuzdan her çıktığında hızını tekrar alsın)
    private void OnEnable()
    {
        rb.linearVelocity = Vector2.down * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().PlayerTakeDamage(damage);
            
            // 5. Destroy YERİNE Havuza Dönüş Kalkanı
            if(gameObject.activeSelf && referencePool != null)
            {
                referencePool.Release(this);
            }
        }
    }

    // DİKKAT: Küçük 'o' harfi Büyük 'O' olarak düzeltildi!
    private void OnBecameInvisible()
    {
        // 5. Destroy YERİNE Havuza Dönüş Kalkanı
        if(gameObject.activeSelf && referencePool != null)
        {
            referencePool.Release(this);
        }
    }
}