using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[Header("Inventory")]
	public GameObject InventoryMenu;
	public bool menuActivated;

	[Space]

	[Header("Item Slot")]
	public ItemSlot[] itemSlot;

	[Header("Item SO")]
	public ItemSO[] itemSO;

	public bool UseItem(string itemName)
	{
		for (int i = 0; i < itemSO.Length; i++)
		{
			if (itemSO[i].itemName == itemName)
			{
				bool usable = itemSO[i].UseItem();
				return usable;
			}
		}
		return false;
	}

	public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
	{
		for (int i = 0; i < itemSlot.Length; i++)
		{
			if (itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
			{
				int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
				if (leftOverItems > 0)
					leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);

				return leftOverItems;
			}
		}
		return quantity;
	}

	public void DeselectAllSlots()
	{
		for (int i = 0; i < itemSlot.Length; i++)
		{
			itemSlot[i].selectedShader.SetActive(false);
			itemSlot[i].thisItemSelected = false;
		}
	}

	public void ActivateInventory()
	{
		if (InventoryMenu.activeInHierarchy == false)
		{
			Time.timeScale = 0;
			InventoryMenu.SetActive(true);
		}
		else
		{
			Time.timeScale = 1;
			InventoryMenu.SetActive(false);
		}
	}
}
