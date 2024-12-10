using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUpgrade : MonoBehaviour
{
    [SerializeField] private int currentHealthUp = 0;
    [SerializeField] private int currentSpeedUp = 0;

    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private Slider healthSlider;
    private Slider speedSlider;
    const string HeartSlider = "Upgrade Health Slider";
    const string SpeedSlider = "Upgrade Speed Slider";

    private CoinManager coinManager;
    private int healthUpgradeCost = 1;
    private int speedUpgradeCost = 1;

    private void Start()
    {
        coinManager = CoinManager.Instance;
        healthSlider = GameObject.Find(HeartSlider).GetComponent<Slider>();
        speedSlider = GameObject.Find(SpeedSlider).GetComponent<Slider>();

    }

    //Increase health
    public void OnClickHealth()
    {
        playerHealth = PlayerHealth.Instance;
        if(coinManager.SpendCoin(healthUpgradeCost))
        {
            UpgradeHealth();
            UpdateHealthSlider();
            healthUpgradeCost += 1;
        }
    }

    private void UpgradeHealth()
    {
        if (currentHealthUp < 10)
        {
            currentHealthUp += 1;
            playerHealth.IncreaseMaxHealth();
        }
    }

    private void UpdateHealthSlider()
    {
        
        healthSlider.maxValue = 10;
        healthSlider.value = currentHealthUp;
    }




    //Increase Speed
    public void OnClickSpeed()
    {
        playerController = PlayerController.Instance;
        if (coinManager.SpendCoin(speedUpgradeCost))
        {
            UpgradeSpeed();
            UpdateSpeedSlider();
            speedUpgradeCost += 1;
        }
    }

    private void UpgradeSpeed()
    {
        if (currentSpeedUp < 10)
        {
            currentSpeedUp += 1;
            playerController.IncreaseMoveSpeed();
        }
    }

    private void UpdateSpeedSlider()
    {
        
        speedSlider.maxValue = 10;
        speedSlider.value = currentSpeedUp;
    }
}
