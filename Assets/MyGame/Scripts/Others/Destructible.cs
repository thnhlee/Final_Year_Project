using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<AttackDamage>() || collision.gameObject.GetComponent<Projectile>())
        {
            GetComponent<ItemSpawner>().DropItem();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
