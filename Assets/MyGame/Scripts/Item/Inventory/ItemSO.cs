using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerObj;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
	public string itemName;
	public StatToChange statToChange = new StatToChange();
	public int amountToChangeStat;

	private PlayerHealth playerHealth;
	public bool UseItem()
	{
		if (statToChange == StatToChange.Potion)
		{
			playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
			if (playerHealth.currentHealth == playerHealth.maxHealth)
			{
				return false;
			}
			else
			{
				playerHealth.currentHealth += amountToChangeStat;
				playerHealth.UpdateHealthSlider();
				return true;
			}
		}
		return false;
	}

	public enum StatToChange
	{
		none,
		Potion
	}
}
