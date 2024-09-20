using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[AddComponentMenu("ThinhLe/EnemyHealth")]


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int Health = 3;
    [SerializeField] private GameObject deathVfx;
    [SerializeField] private float knockBackThrust = 15f;

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
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(hitFlash.FlashRoutine());
        StartCoroutine(CheckDeadRoutine());
    }

    private IEnumerator CheckDeadRoutine()
    {
        yield return new WaitForSeconds(hitFlash.GetRestoreMatTime());
        Dead();
    }

    public void Dead()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
