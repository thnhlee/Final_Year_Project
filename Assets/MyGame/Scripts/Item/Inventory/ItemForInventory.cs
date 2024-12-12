using UnityEngine;

public class ItemForInventory : MonoBehaviour
{
	[SerializeField] private string itemName;
	[SerializeField] private int quantity;
	[SerializeField] private Sprite sprite;

	private string text;

	[TextArea]
	[SerializeField] string itemDescription;

	private InventoryManager inventoryManager;
	private void Start()
	{
		inventoryManager = GameObject.Find("UI").GetComponent<InventoryManager>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
			if (leftOverItems <= 0)
				Destroy(gameObject);
			else
				quantity = leftOverItems;
		}
	}
}
