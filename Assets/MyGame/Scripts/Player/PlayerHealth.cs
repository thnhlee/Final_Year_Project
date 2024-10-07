using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/PlayerHealth")]

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrust = 5f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockBack;
    private HitFlash hitFlash;

    protected override void Awake()
    {
        base.Awake();

        hitFlash = GetComponent<HitFlash>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI2 enemy = collision.gameObject.GetComponent<EnemyAI2>();

        if (enemy)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void Healing()
    {
        currentHealth += 1;
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage)
        {
            return;
        }

        ScreenShake.Instance.ShakeScreen();

        knockBack.GetKnockedBack(hitTransform.gameObject.transform, knockBackThrust);
        StartCoroutine(hitFlash.FlashRoutine());

        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryTimeRoutine());
    }

    private IEnumerator DamageRecoveryTimeRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
