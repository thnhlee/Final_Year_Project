using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/EnemyHealth")]


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int Health = 3;

    private int currentHealth;
    private KnockBack knockBack;
    private HitFlash hitFlash;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        hitFlash = GetComponent<HitFlash>();
    }
    private void Start()
    {
        currentHealth = Health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform, 15f);
        StartCoroutine(hitFlash.FlashRoutine());
    }

    public void Dead()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
