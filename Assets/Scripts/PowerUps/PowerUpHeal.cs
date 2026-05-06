using UnityEngine;

public class PowerUpHeal : MonoBehaviour
{
    [SerializeField] private float healAmount;

    [SerializeField] private AudioClip clipToPlay;
     

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerStats player=collision.GetComponent<PlayerStats>();
            player.AddHealth((int)healAmount);
            AudioSource.PlayClipAtPoint(clipToPlay, transform.position, 1f);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }   
}
