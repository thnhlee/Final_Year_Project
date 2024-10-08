using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[AddComponentMenu("ThinhLe/PlayerHealth")]

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrust = 5f;
    [SerializeField] private float damageRecoveryTime = 1f;

    public bool isDead {  get; private set; }

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockBack;
    private HitFlash hitFlash;

    const string HeartSlider = "Heart Slider";
    const string BaseRespawn = "Base";
    readonly int DeadAnim = Animator.StringToHash("isDead");


    protected override void Awake()
    {
        base.Awake();

        hitFlash = GetComponent<HitFlash>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        UpdateHealthSlider();
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
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
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
        UpdateHealthSlider();
        PlayerDeath();
    }

    private IEnumerator DamageRecoveryTimeRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void PlayerDeath()
    {
        if(currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);

            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DeadAnim);

        }
    }

    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find(HeartSlider).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
