using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[AddComponentMenu("ThinhLe/EnemyHealth")]


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int Health = 3;
    [SerializeField] private GameObject deathVfx;
    [SerializeField] private float knockBackThrust = 15f;
    private GameObject gameWin; 
    private GameObject ActivehealthSlider;

    private int currentHealth;
    private KnockBack knockBack;
    private HitFlash hitFlash;
    private Slider healthSlider;



    const string GameWinUI = "GameWin";
    const string HeartSlider = "Boss Heart Slider";

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        hitFlash = GetComponent<HitFlash>();
    }

    private void FindUIElements()
    {
        if (gameWin == null)
        {
            gameWin = GameObject.FindObjectsOfType<GameObject>(true).FirstOrDefault(obj => obj.name == GameWinUI);
        }

        if (ActivehealthSlider == null)
        {
            ActivehealthSlider = GameObject.Find(HeartSlider);
        }
    }

    public void Start()
    {
        currentHealth = Health;
        FindUIElements();
        if (gameObject.CompareTag("Bosses"))
        {
            ActivehealthSlider.SetActive(true);
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(hitFlash.FlashRoutine());
        StartCoroutine(CheckDeadRoutine());
        if (gameObject.CompareTag("Bosses"))
        {
            UpdateHealthSlider();
        }
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
            GetComponent<ItemSpawner>().DropItem();

            if (gameObject.CompareTag("Bosses"))
            {
                FindUIElements();
                Time.timeScale = 0;
                if (ActivehealthSlider != null)
                {
                    ActivehealthSlider.SetActive(false);
                }
                if (gameWin != null)
                {
                    gameWin.SetActive(true);
                }
            }

            Destroy(gameObject);
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HeartSlider).GetComponent<Slider>();
        }

        healthSlider.maxValue = Health;
        healthSlider.value = currentHealth;
    }
}
