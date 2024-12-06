using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    private int collisionCount = 0; 
    private const int requiredCollisions = 3; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if ((collision.gameObject.GetComponent<AttackDamage>() || collision.gameObject.GetComponent<Projectile>()) && player)
        {
            if (gameObject.CompareTag("Temp"))
            {
                  collisionCount++; 
                  if (collisionCount >= requiredCollisions)
                      {
                            GetComponent<ItemSpawner>().DropItem();
                            Instantiate(destroyVFX, transform.position, Quaternion.identity);
                           Destroy(gameObject);
                           Debug.Log("Hehe");
                      }
            }
        }
    
    }
}