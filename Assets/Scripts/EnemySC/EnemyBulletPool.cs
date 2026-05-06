using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    // Singleton: Diğer kodların bu havuza direkt isimiyle ulaşmasını sağlar!
    public static EnemyBulletPool Instance; 

    [SerializeField] private PurpleBullet bulletPrefab;
    private ObjectPool<PurpleBullet> pool;

    private void Awake()
    {
        Instance = this; // Kendini merkeze kaydeder
        pool = new ObjectPool<PurpleBullet>(CreateBullet, OnTake, OnReturn, OnDestroyBullet, true, 20, 100);
    }

    private PurpleBullet CreateBullet()
    {
        PurpleBullet bullet = Instantiate(bulletPrefab);
        bullet.SetPool(pool);
        return bullet;
    }

    private void OnTake(PurpleBullet bullet) => bullet.gameObject.SetActive(true);
    private void OnReturn(PurpleBullet bullet) => bullet.gameObject.SetActive(false);
    private void OnDestroyBullet(PurpleBullet bullet) => Destroy(bullet.gameObject);

    // Düşmanların mermi istemek için kullanacağı komut
    public PurpleBullet GetBullet()
    {
        return pool.Get();
    }
}