/*
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    private Camera mainCam;
    private float maxLeft;
    private float maxRight;
    private float yPos;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemy;
    
    private float enemyTimer;
    [Space(15)]
    [SerializeField] private float enemySpawnTime;
    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private WinCondition winCon;

    void Start()
    {

        mainCam = Camera.main;
        StartCoroutine(SetBoundaries());

    }

   
    void Update()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        enemyTimer+=Time.deltaTime; 
        if(enemyTimer>enemySpawnTime)
        {
            int i = Random.Range(0, enemy.Length);
            Instantiate(enemy[i], new Vector3(Random.Range(maxLeft, maxRight), yPos, 0), Quaternion.identity);
            enemyTimer=0f;
        }
    }

    private IEnumerator SetBoundaries()
    {
        yield return new WaitForSeconds(0.4f);

        maxLeft= mainCam.ViewportToWorldPoint(new Vector2(0.1f,0)).x;
        maxRight= mainCam.ViewportToWorldPoint(new Vector2(0.9f,0)).x;
        yPos=mainCam.ViewportToWorldPoint(new Vector2(0,1.1f)).y;
    }

    private void OnDisable()
    {
        if(winCon.canSpawnBoss==false)
        {
            return;
        }
        if(bossPrefab!=null)
        {
            Vector2 spawnPos = mainCam.ViewportToWorldPoint(new Vector2(0.5f,1.2f));
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        }
    }
    
}
*/

using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.Pool; // Havuz kütüphanesi eklendi

public class EnemySpawner : MonoBehaviour
{
    private Camera mainCam;
    private float maxLeft;
    private float maxRight;
    private float yPos;

    [Header("Enemy Prefabs")]
    // GameObject yerine direkt Enemy scriptini alıyoruz
    [SerializeField] private Enemy[] enemyPrefabs; 
    
    private float enemyTimer;
    [Space(15)]
    [SerializeField] private float enemySpawnTime;
    
    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private WinCondition winCon;

    // Havuzlar Dizisi
    private ObjectPool<Enemy>[] enemyPools;

    private void Awake()
    {
        enemyPools = new ObjectPool<Enemy>[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            int index = i; 
            enemyPools[i] = new ObjectPool<Enemy>(
                createFunc: () => CreateEnemy(index), 
                actionOnGet: OnTakeEnemy, 
                actionOnRelease: OnReturnEnemy, 
                actionOnDestroy: OnDestroyEnemy, 
                collectionCheck: true, 
                defaultCapacity: 5, 
                maxSize: 20
            );
        }
    }

    void Start()
    {
        mainCam = Camera.main;
        StartCoroutine(SetBoundaries());
    }

    void Update()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        enemyTimer += Time.deltaTime; 
        if(enemyTimer > enemySpawnTime)
        {
            int i = Random.Range(0, enemyPrefabs.Length);
            
            // Havuzdan düşmanı çek
            Enemy obj = enemyPools[i].Get();
            obj.transform.position = new Vector3(Random.Range(maxLeft, maxRight), yPos, 0);
            obj.transform.rotation = Quaternion.identity; // Düşmanlar düz doğar
            
            enemyTimer = 0f;
        }
    }

    // --- HAVUZ FONKSİYONLARI ---

    private Enemy CreateEnemy(int index)
    {
        Enemy newEnemy = Instantiate(enemyPrefabs[index]);
        newEnemy.SetPool(enemyPools[index]); 
        return newEnemy;
    }

    private void OnTakeEnemy(Enemy enemyObj)
    {
        enemyObj.gameObject.SetActive(true);
    }

    private void OnReturnEnemy(Enemy enemyObj)
    {
        enemyObj.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(Enemy enemyObj)
    {
        Destroy(enemyObj.gameObject);
    }

    private IEnumerator SetBoundaries()
    {
        yield return new WaitForSeconds(0.4f);
        maxLeft = mainCam.ViewportToWorldPoint(new Vector2(0.1f, 0)).x;
        maxRight = mainCam.ViewportToWorldPoint(new Vector2(0.9f, 0)).x;
        yPos = mainCam.ViewportToWorldPoint(new Vector2(0, 1.1f)).y;
    }

    private void OnDisable()
    {
        // NullReference kalkanları eklendi (Boss için)
        if(winCon == null || winCon.canSpawnBoss == false)
        {
            return;
        }
        if(bossPrefab != null && mainCam != null)
        {
            Vector2 spawnPos = mainCam.ViewportToWorldPoint(new Vector2(0.5f, 1.2f));
            Instantiate(bossPrefab, spawnPos, Quaternion.identity); // Boss havuza girmez, Instantiate kalır
        }
    }
}