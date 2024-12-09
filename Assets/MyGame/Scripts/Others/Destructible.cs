using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    
    private int collisionCount = 0; 
    private const int requiredCollisions = 3;



    private void OnTriggerEnter2D(Collider2D collision)
    {


        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        
        Light2D playerLight = PlayerController.Instance.gameObject.GetComponent<Light2D>();

        if (collision.gameObject.GetComponent<AttackDamage>() || collision.gameObject.GetComponent<Projectile>() && player)
        {
            if (gameObject.CompareTag("Temp"))
            {
                collisionCount++;
                if (collisionCount >= requiredCollisions)
                {
                    GetComponent<ItemSpawner>().DropItem();
                    Instantiate(destroyVFX, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    playerLight.enabled = true;
                }
            }
            else
            {
                GetComponent<ItemSpawner>().DropItem();
                Instantiate(destroyVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}