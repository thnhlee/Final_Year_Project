using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject detroyVFX;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<AttackDamage>())
        {
            Instantiate(detroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
