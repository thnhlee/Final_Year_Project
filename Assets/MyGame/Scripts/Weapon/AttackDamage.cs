using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/AttackDamage")]

public class AttackDamage : MonoBehaviour
{
    [SerializeField] private int swordDamage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(swordDamage);
    }
}
