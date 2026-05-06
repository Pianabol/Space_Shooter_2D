using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    [SerializeField] private Animator anim;

    [SerializeField] private Image healthFill;   
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Shield shield;

    private PlayerShooting playerShooting;

    private float currentHealth;

    private bool canPlayAnim = true;

    void Start()
    {
        currentHealth = maxHealth;
        healthFill.fillAmount=currentHealth / maxHealth;    
        EndGameManager.endManager.gameOver= false;  
        playerShooting = GetComponent<PlayerShooting>();

    }
     void Update()
    {
        
    }

    public void PlayerTakeDamage(float damage)
    {
        if(shield.protection)
        {
            return;
        }
        currentHealth -= damage;
        healthFill.fillAmount=currentHealth / maxHealth; 
        if(canPlayAnim)
        {
            anim.SetTrigger("Damage");   
            StartCoroutine(AntiSpamAnimation());    
        }
        playerShooting.DecreaseUpgradeLevel(1);
        if (currentHealth <= 0)
        {
            EndGameManager.endManager.gameOver= true;
            EndGameManager.endManager.StartResolveSequence();

            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void AddHealth(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth>maxHealth)
        {
            currentHealth=maxHealth;
        }
        healthFill.fillAmount=currentHealth / maxHealth;    
    }
     
    private IEnumerator AntiSpamAnimation()
    {
        canPlayAnim = false;
        yield return new WaitForSeconds(0.15f); // Animasyonun süresine göre bekleme süresi
        canPlayAnim = true;
    }
}
