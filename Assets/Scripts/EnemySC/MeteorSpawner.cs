/*
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] meteor;
    [SerializeField] private float spawnTime;    
    
    private float timer=0f;
    private int i;
    private Camera mainCam;
    private float maxLeft;
    private float maxRight;
    private float yPos;



    void Start()
    {
        //
        mainCam = Camera.main;
        StartCoroutine(SetBoundaries());

    }
    void Update()
    {
        timer+=Time.deltaTime; 
        if(timer>spawnTime)
        {
            i = Random.Range(0, meteor.Length);
            GameObject obj= Instantiate(meteor[i], new Vector3(Random.Range(maxLeft, maxRight), yPos, -5), Quaternion.Euler(0,0,Random.Range(0,360)));
            float size= Random.Range(0.7f,1.2f);
            obj.transform.localScale=new Vector3(size,size,1);
            timer=0f;
        }
    }
    
    private IEnumerator SetBoundaries()
    {
        yield return new WaitForSeconds(0.4f);

        maxLeft= mainCam.ViewportToWorldPoint(new Vector2(0.1f,0)).x;
        maxRight= mainCam.ViewportToWorldPoint(new Vector2(0.9f,0)).x;
        yPos=mainCam.ViewportToWorldPoint(new Vector2(0,1.1f)).y;
    }
}
*/

using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.Pool; // Havuz kütüphanesi eklendi!

public class MeteorSpawner : MonoBehaviour
{
    // Artık GameObject yerine direkt Meteor scriptini (bileşenini) alıyoruz ki GetComponent ile uğraşmayalım.
    [SerializeField] private Meteor[] meteorPrefabs; 
    [SerializeField] private float spawnTime;    
    
    private float timer=0f;
    private Camera mainCam;
    private float maxLeft;
    private float maxRight;
    private float yPos;

    // SİHRİN BAŞLADIĞI YER: Tek bir havuz değil, havuzlar dizisi!
    private ObjectPool<Meteor>[] meteorPools; 

    private void Awake()
    {
        // Kaç tane meteor prefabımız varsa, o kadar havuz açıyoruz.
        meteorPools = new ObjectPool<Meteor>[meteorPrefabs.Length];

        for (int i = 0; i < meteorPrefabs.Length; i++)
        {
            // 'index' değişkenini döngü içinde özel olarak tanımlamamız lazım ki 
            // Create Func içinde hangi meteoru ürettiğini karıştırmasın.
            int index = i; 
            
            meteorPools[i] = new ObjectPool<Meteor>(
                createFunc: () => CreateMeteor(index), 
                actionOnGet: OnTakeMeteor, 
                actionOnRelease: OnReturnMeteor, 
                actionOnDestroy: OnDestroyMeteor, 
                collectionCheck: true, 
                defaultCapacity: 5, // Meteorlar yavaş çıktığı için kapasiteyi küçük tutabiliriz
                maxSize: 15
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
        timer += Time.deltaTime; 
        if(timer > spawnTime)
        {
            SpawnMeteor();
            timer = 0f;
        }
    }

    private void SpawnMeteor()
    {
        // Rastgele bir havuz (meteor tipi) seç
        int randomIndex = Random.Range(0, meteorPrefabs.Length);
        
        // O havuzdan bir meteor çek
        Meteor obj = meteorPools[randomIndex].Get();
        
        // Eskiden Instantiate içinde yaptığın o harika pozisyon, rotasyon ve boyut ayarlarını şimdi burada, havuzdan çektikten sonra yapıyoruz:
        obj.transform.position = new Vector3(Random.Range(maxLeft, maxRight), yPos, -5);
        obj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        
        float size = Random.Range(0.7f, 1.2f);
        obj.transform.localScale = new Vector3(size, size, 1);
    }

    // --- HAVUZ FONKSİYONLARI ---

    private Meteor CreateMeteor(int index)
    {
        Meteor newMeteor = Instantiate(meteorPrefabs[index]);
        // Tıpkı mermideki gibi, meteora kendi havuzunun referansını veriyoruz
        newMeteor.SetPool(meteorPools[index]); 
        return newMeteor;
    }

    private void OnTakeMeteor(Meteor meteor)
    {
        meteor.gameObject.SetActive(true);
    }

    private void OnReturnMeteor(Meteor meteor)
    {
        meteor.gameObject.SetActive(false);
    }

    private void OnDestroyMeteor(Meteor meteor)
    {
        Destroy(meteor.gameObject);
    }

    private IEnumerator SetBoundaries()
    {
        yield return new WaitForSeconds(0.4f);
        maxLeft = mainCam.ViewportToWorldPoint(new Vector2(0.1f, 0)).x;
        maxRight = mainCam.ViewportToWorldPoint(new Vector2(0.9f, 0)).x;
        yPos = mainCam.ViewportToWorldPoint(new Vector2(0, 1.1f)).y;
    }
}