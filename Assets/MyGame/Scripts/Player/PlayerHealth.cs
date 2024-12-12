using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
[AddComponentMenu("ThinhLe/PlayerHealth")]

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] public int maxHealth = 3;
    [SerializeField] private float knockBackThrust = 5f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private GameObject gameOverUI; 
    [SerializeField] private GameObject gameWin;

    public bool isDead { get; private set; }

    private const int maxHealthIncrease = 13;
    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockBack;
    private HitFlash hitFlash;

    const string HeartSlider = "Heart Slider";
    const string BaseRespawn = "Base";
    readonly int DeadAnim = Animator.StringToHash("isDead");
    private const string BossHealthSliderUI = "Boss Heart Slider";
    

    protected override void Awake()
    {
        base.Awake();
        hitFlash = GetComponent<HitFlash>();
        knockBack = GetComponent<KnockBack>();
    }

    public void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        gameOverUI.SetActive(false);
        gameWin.SetActive(false);
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

        if (hitTransform.CompareTag("Bosses"))
        {
            knockBack.GetKnockedBack(hitTransform.gameObject.transform, 30);
        }
        else
        {
            knockBack.GetKnockedBack(hitTransform.gameObject.transform, knockBackThrust);
        }
        
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

    public void PlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DeadAnim);
            // Pause game
            // Time.timeScale = 0;
            gameOverUI.SetActive(true);

            GameObject bossHealthSlider = GameObject.Find(BossHealthSliderUI);
            if (bossHealthSlider != null)
            {
                bossHealthSlider.SetActive(false);
            }
        }
    }

    public void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HeartSlider).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void IncreaseMaxHealth()
    {
        if(maxHealth < maxHealthIncrease)
        maxHealth += 1;
        currentHealth += 1;
        UpdateHealthSlider(); 
    }
}
