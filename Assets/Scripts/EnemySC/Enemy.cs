/*
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;


public class Enemy : MonoBehaviour
{

    [SerializeField] private float health;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float damage;
    [SerializeField] protected GameObject explosionPrefab;

    [SerializeField] protected Animator anim;
    
    [Header("Score"), SerializeField] protected int scoreValue;


    
    
    public void TakeDamage(float dmg)
    {
        health-=dmg;
        HurtSequence();
        if(health<=0)
        {
            //Destroy(gameObject);
            DeathSequence();
        }  
    } 

    public virtual void HurtSequence()
    {
        //hurt
    }
    
    public virtual void DeathSequence()
    {
        EndGameManager.endManager.UpdateScore(scoreValue);
    }
}
*/

using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.Pool; // Havuz kütüphanesi

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    private float defaultHealth; // Can yenileme için eklendi

    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float damage;
    [SerializeField] protected GameObject explosionPrefab;
    [SerializeField] protected Animator anim;
    
    [Header("Score"), SerializeField] protected int scoreValue;

    // Alt sınıflar (Green/Purple) erişebilsin diye "protected" yaptık
    protected ObjectPool<Enemy> referencePool;

    private void Awake()
    {
        // Inspector'dan girdiğin o ilk can değerini aklında tutar
        defaultHealth = health; 
    }

    protected virtual void OnEnable()
    {
        // Havuzdan her uyandığında canını fuller!
        health = defaultHealth; 
    }

    public void SetPool(ObjectPool<Enemy> pool)
    {
        referencePool = pool;
    }

    // Alt sınıflar Destroy yerine artık bu metodu çağıracak
    public void ReturnToPool()
    {
        if(gameObject.activeSelf && referencePool != null)
        {
            referencePool.Release(this);
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        HurtSequence();
        if(health <= 0)
        {
            DeathSequence();
        }  
    } 

    public virtual void HurtSequence()
    {
        //hurt
    }
    
    public virtual void DeathSequence()
    {
        EndGameManager.endManager.UpdateScore(scoreValue);
    }
}