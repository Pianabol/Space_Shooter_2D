using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;


public class PlayerShooting : MonoBehaviour
{

    [SerializeField] private GameObject laserBullet;
    
    [SerializeField] private float shootingInterval;
    
    [Header("Basic Attack")]
    [SerializeField] private Transform basicShootPoint;
    
    [Header("Upgrade Points")]
    [SerializeField] private Transform leftCanon;
    [SerializeField] private Transform rightCanon;
    [SerializeField] private Transform secondLeftCanon;
    [SerializeField] private Transform secondRightCanon;

    [Header("Upgrade Rotation Points")]
    [SerializeField] private Transform leftRotationCanon;
    [SerializeField] private Transform rightRotationCanon;

    [SerializeField] private AudioSource source;

    private int upgradeLevel=0;
    private float intervalReset;
    
    private ObjectPool<LaserBullet> pool;

    private void Awake()
    {
        pool = new ObjectPool<LaserBullet>(CreatePoolObj, OnTakeBulletFromPool, OnReturnBulletFromPool, OnDestroyPoolObj, true,10,30);
    }
    void Start()
    {
        intervalReset=shootingInterval;
    }

    private void OnDestroyPoolObj(LaserBullet bullet) 
    {
        Destroy(bullet.gameObject);
    }

    private LaserBullet CreatePoolObj()
    {
        LaserBullet bullet= Instantiate(laserBullet, transform.position, transform.rotation).GetComponent<LaserBullet>();
        bullet.SetPool(pool);
        return bullet;
    }
    private void OnReturnBulletFromPool(LaserBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnTakeBulletFromPool(LaserBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }


    void Update()
    {
        shootingInterval-=Time.deltaTime;
        if(shootingInterval<=0)
        {
            Shoot();
            shootingInterval=intervalReset;
        }
    }
    public void IncreaseUpgradeLevel(int increaseAmount)
    {
        upgradeLevel += increaseAmount;
        if(upgradeLevel>4)
        {
            upgradeLevel=4;
        }
    }

    public void DecreaseUpgradeLevel(int decreaseAmount)
    {
        upgradeLevel -= decreaseAmount;
        if(upgradeLevel<0)
        {
            upgradeLevel=0;
        }
    }
    private void Shoot()
    {
        source.Play();
        //Instantiate(laserBullet, basicShootPoint.position, Quaternion.identity);
        switch(upgradeLevel)
        {
            case 0:
                //Instantiate(laserBullet, basicShootPoint.position, Quaternion.identity);
                pool.Get().transform.position=basicShootPoint.position;
                break;
            case 1:
                //Instantiate(laserBullet, leftCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, rightCanon.position, Quaternion.identity);
                pool.Get().transform.position=leftCanon.position;
                pool.Get().transform.position=rightCanon.position;
                break;
            case 2:
                //Instantiate(laserBullet, basicShootPoint.position, Quaternion.identity);
                //Instantiate(laserBullet, leftCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, rightCanon.position, Quaternion.identity);
                pool.Get().transform.position=basicShootPoint.position;
                pool.Get().transform.position=leftCanon.position;
                pool.Get().transform.position=rightCanon.position;
                break;
            case 3:
                //Instantiate(laserBullet, basicShootPoint.position, Quaternion.identity);
                //Instantiate(laserBullet, leftCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, rightCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, secondLeftCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, secondRightCanon.position, Quaternion.identity);
                pool.Get().transform.position=basicShootPoint.position;
                pool.Get().transform.position=leftCanon.position;
                pool.Get().transform.position=rightCanon.position;
                pool.Get().transform.position=secondLeftCanon.position;
                pool.Get().transform.position=secondRightCanon.position;
                break;
            case 4:
                //Instantiate(laserBullet, basicShootPoint.position, Quaternion.identity);
                //Instantiate(laserBullet, leftCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, rightCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, secondLeftCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, secondRightCanon.position, Quaternion.identity);
                //Instantiate(laserBullet, leftRotationCanon.position, leftRotationCanon.rotation);
                //Instantiate(laserBullet, rightRotationCanon.position, rightRotationCanon.rotation);
                pool.Get().transform.position=basicShootPoint.position;
                pool.Get().transform.position=leftCanon.position;
                pool.Get().transform.position=rightCanon.position;
                pool.Get().transform.position=secondLeftCanon.position;
                pool.Get().transform.position=secondRightCanon.position;

                LaserBullet bulletOne= pool.Get();
                bulletOne.transform.position=leftRotationCanon.position;
                bulletOne.transform.rotation=leftRotationCanon.rotation;
                bulletOne.SetDirectionAndSpeed();

                LaserBullet bulletTwo= pool.Get();
                bulletTwo.transform.position=rightRotationCanon.position;
                bulletTwo.transform.rotation=rightRotationCanon.rotation;
                bulletTwo.SetDirectionAndSpeed();

                break;
            default: 
                Debug.Log("Something went wrong with the shooting system, check the upgrade level");
                break;    
            
        }

    }
}
